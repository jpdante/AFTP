using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;
using AFTPLib.Exceptions;
using AFTPLib.Protocol.Args;
using AFTPLib.Protocol.Commands;
using AFTPLib.Protocol.Commands.Handshake;
using AFTPLib.Protocol.Commands.List;
using AFTPLib.Protocol.Commands.Models;
using AFTPLib.Protocol.Commands.Other;
using ProtoBuf;

namespace AFTPLib.Protocol {
    public class AftpConnection {

        private Stream _stream;
        private readonly bool _isServer;
        private readonly ConnectionSettings _settings;
        private readonly ConnectionInfo _info;
        private readonly int _defaultTimeout;
        private readonly Timer _timer;
        private bool _isAuthenticated;
        private bool _continueReadStream;
        private int _initializationStatus;
        private int _timeout;
        public readonly List<ProtoStream> AsyncResponseBuffer;

        #region Events

        public delegate void CheckVersionHandler(object sender, CheckVersionEventArgs args);
        public CheckVersionHandler OnCheckVersion;

        public delegate Stream RequestSecureStreamHandler(object sender, Stream stream, bool selfSigned);
        public RequestSecureStreamHandler OnRequestSecureStream;

        public delegate void AuthenticationHandler(object sender, AuthenticationRequestEventArgs args);
        public AuthenticationHandler OnAuthenticationRequest;

        public delegate void AuthenticationResultHandler(object sender, AuthenticationResponseEventArgs args);
        public AuthenticationResultHandler OnAuthenticationResult;

        public delegate void FinishHandshakeHandler(object sender);
        public FinishHandshakeHandler OnHandshakeFinish;

        public delegate void ErrorHandler(object sender, Exception ex);
        public ErrorHandler OnErrorOccurs;

        #endregion

        public AftpConnection(Stream stream, ConnectionSettings settings, bool isServer = false, int timeout = 10) {
            _stream = stream;
            _isServer = isServer;
            _settings = settings;
            _info = new ConnectionInfo();
            AsyncResponseBuffer = new List<ProtoStream>();
            if (!_isServer) return;
            _defaultTimeout = timeout;
            _isAuthenticated = false;
            _initializationStatus = 0;
            _timer = new Timer {
                Interval = 1000
            };
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e) {
            _timeout--;
            if (_timeout <= 0 && !_isAuthenticated) {
                HardShutdown(EndConnectionReason.Unknown, new HandshakeTimeoutException());
                _timer.Stop();
            } else if (_timeout <= 0 && _isAuthenticated) {
                _timer.Stop();
            }
        }

        public void Handshake() {
            if (_isServer) {
                _timeout = _defaultTimeout;
                _timer.Start();
                ServerHandshake();
            } else ClientHandshake();
        }

        public void SendCommand(ProtoStream command) {
            Console.WriteLine("Sending 8");
            Serializer.SerializeWithLengthPrefix(_stream, command, PrefixStyle.Fixed32);
        }

        private void HardShutdown(EndConnectionReason reason, Exception error) {
            _continueReadStream = false;
            _isAuthenticated = false;
            if (_stream.CanWrite) Serializer.SerializeWithLengthPrefix(_stream, new EndConnection(reason), PrefixStyle.Fixed32);
            _stream.Close();
            _stream.Dispose();
            OnErrorOccurs(this, error);
        }

/* Commands by ID
 * 0: StartHandshake
 * 1: GetVersion
 * 2: SetSetting
 * 3: RequestEncryption
 * 4: FinishEncryption
 * 5: FinishHandshake
 * 6: RequestAuthentication
 * 7: AuthenticationResponse
 */

        private void ServerHandshake() {
            ReadStream<StartHandshake>();
            Serializer.SerializeWithLengthPrefix(_stream, new StartHandshake(), PrefixStyle.Fixed32);
            var version = ReadStream<GetVersion>();
            _info.Version = version.Version;
            _info.Software = version.Software;
            var checkVersionArgs = new CheckVersionEventArgs(_info.Version, _info.Software);
            OnCheckVersion(this, checkVersionArgs);
            if (checkVersionArgs.Cancel) {
                Serializer.SerializeWithLengthPrefix(_stream, new EndConnection(), PrefixStyle.Fixed32);
                return;
            }
            _initializationStatus = 1;
            Serializer.SerializeWithLengthPrefix(_stream, new GetVersion(_settings.SentVersion, _settings.SentSoftware), PrefixStyle.Fixed32);
            Serializer.SerializeWithLengthPrefix(_stream, new SetSetting(Commands.Other.ConnectionSettings.UseEncryption, _settings.UseEncryption.ToString()), PrefixStyle.Fixed32);
            Serializer.SerializeWithLengthPrefix(_stream, new SetSetting(Commands.Other.ConnectionSettings.EncryptionType, _settings.ConnectionEncryptionType.ToString()), PrefixStyle.Fixed32);
            Serializer.SerializeWithLengthPrefix(_stream, new SetSetting(Commands.Other.ConnectionSettings.SelfSignedCertificate, _settings.CertificateSelfSigned.ToString()), PrefixStyle.Fixed32);
            Serializer.SerializeWithLengthPrefix(_stream, new SetSetting(Commands.Other.ConnectionSettings.PasswordEncryptionType, _settings.PasswordEncryptionType.ToString()), PrefixStyle.Fixed32);
            if (_settings.UseEncryption) {
                Serializer.SerializeWithLengthPrefix(_stream, new RequestEncryption(), PrefixStyle.Fixed32);
                _stream = OnRequestSecureStream(this, _stream, _settings.CertificateSelfSigned);
            } else {
                Serializer.SerializeWithLengthPrefix(_stream, new FinishHandshake(), PrefixStyle.Fixed32);
                _initializationStatus = 2;
            }
            _continueReadStream = true;
            while (_stream.CanRead && _continueReadStream) {
                var protoStream = ReadStream();
                Console.WriteLine(protoStream.CommandId);
                switch (protoStream.CommandId) {
                    case 2:
                        var setSetting = (SetSetting)protoStream;
                        if (_initializationStatus == 1) {
                            //Future Usage
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 4:
                        if (_initializationStatus == 1) {
                            Serializer.SerializeWithLengthPrefix(_stream, new FinishHandshake(), PrefixStyle.Fixed32);
                            _initializationStatus = 2;
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 5:
                        if (_initializationStatus == 2) {
                            OnHandshakeFinish(this);
                            _initializationStatus = 3;
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 6:
                        if (_initializationStatus == 3) {
                            var userAuthentication = (RequestAuthentication)protoStream;
                            var authenticationRequest = new AuthenticationRequestEventArgs(userAuthentication.User, userAuthentication.Password, _settings.PasswordEncryptionType);
                            OnAuthenticationRequest(this, authenticationRequest);
                            Serializer.SerializeWithLengthPrefix(_stream, new AuthenticationResponse(authenticationRequest.Success), PrefixStyle.Fixed32);
                            _isAuthenticated = authenticationRequest.Success;
                            if (_isAuthenticated) {
                                Task.Factory.StartNew(ConnectionLoop);
                                return;
                            }
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    default:
                        HardShutdown(EndConnectionReason.InvalidCommand, new UnknownCommandException(protoStream.CommandId));
                        break;
                }
            }
        }

        private void ClientHandshake() {
            Serializer.SerializeWithLengthPrefix(_stream, new StartHandshake(), PrefixStyle.Fixed32);
            ReadStream<StartHandshake>();
            Serializer.SerializeWithLengthPrefix(_stream, new GetVersion(_settings.SentVersion, _settings.SentSoftware), PrefixStyle.Fixed32);
            var version = ReadStream<GetVersion>();
            _info.Version = version.Version;
            _info.Software = version.Software;
            var checkVersionArgs = new CheckVersionEventArgs(_info.Version, _info.Software);
            OnCheckVersion(this, checkVersionArgs);
            if (checkVersionArgs.Cancel) {
                Serializer.SerializeWithLengthPrefix(_stream, new EndConnection(), PrefixStyle.Fixed32);
            }
            _initializationStatus = 1;
            _continueReadStream = true;
            while (_stream.CanRead && _continueReadStream) {
                var protoStream = ReadStream();
                switch (protoStream.CommandId) {
                    case 2:
                        var setSetting = (SetSetting)protoStream;
                        if (_initializationStatus == 1) {
                            switch (setSetting.Setting) {
                                case (byte)Commands.Other.ConnectionSettings.UseEncryption:
                                    _settings.UseEncryption = bool.Parse(setSetting.Value);
                                    break;
                                case (byte)Commands.Other.ConnectionSettings.EncryptionType:
                                    ConnectionEncryptionType.TryParse(setSetting.Value, true, out _settings.ConnectionEncryptionType);
                                    break;
                                case (byte)Commands.Other.ConnectionSettings.SelfSignedCertificate:
                                    _settings.CertificateSelfSigned = bool.Parse(setSetting.Value);
                                    break;
                                case (byte)Commands.Other.ConnectionSettings.PasswordEncryptionType:
                                    PasswordEncryptionType.TryParse(setSetting.Value, true, out _settings.PasswordEncryptionType);
                                    break;
                            }
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 3:
                        if (_initializationStatus == 1) {
                            _stream = OnRequestSecureStream(this, _stream, _settings.CertificateSelfSigned);
                            Serializer.SerializeWithLengthPrefix(_stream, new FinishEncryption(), PrefixStyle.Fixed32);
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 5:
                        if (_initializationStatus == 1) {
                            Serializer.SerializeWithLengthPrefix(_stream, new FinishHandshake(), PrefixStyle.Fixed32);
                            OnHandshakeFinish(this);
                            _initializationStatus = 3;
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 7:
                        if (_initializationStatus == 3) {
                            var authenticationResponse = (AuthenticationResponse)protoStream;
                            OnAuthenticationResult(this, new AuthenticationResponseEventArgs(authenticationResponse.Success));
                            _continueReadStream = true;
                            Task.Factory.StartNew(ConnectionLoop);
                            return;
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    default:
                        HardShutdown(EndConnectionReason.InvalidCommand, new UnknownCommandException(protoStream.CommandId));
                        break;
                }
            }
        }

        public void ConnectionLoop() {
            Console.WriteLine("Continue :");
            _continueReadStream = true;
            while (_stream.CanRead && _continueReadStream) {
                var protoStream = ReadStream();
                Console.WriteLine("> " + protoStream.CommandId);
                switch (protoStream.CommandId) {
                    case 8:
                        if (!_isServer) {
                            HardShutdown(EndConnectionReason.InvalidCommand, new UnknownCommandException(protoStream.CommandId));
                            break;
                        }
                        var request = (RequestDirectoryList)protoStream;
                        var dir = new DirectoryInfo(request.Directory);
                        var entries = new List<DirectoryEntry>();
                        try { entries.AddRange(dir.GetFiles("*").Select(fileInfo => new DirectoryEntry(fileInfo.FullName, true, fileInfo.Length, 0, 777, "Unknown"))); } catch { }
                        entries.AddRange(dir.GetDirectories().Select(directoryInfo => new DirectoryEntry(directoryInfo.FullName, false, -1, 0, 0, "Unknown")));
                        Serializer.SerializeWithLengthPrefix(_stream, new DirectoryListResponse(true, null, entries.ToArray(), request.Guid), PrefixStyle.Fixed32);
                        break;
                    case 9:
                        if (_isServer) {
                            HardShutdown(EndConnectionReason.InvalidCommand, new UnknownCommandException(protoStream.CommandId));
                            break;
                        }
                        AsyncResponseBuffer.Add(protoStream);
                        break;
                }
            }
        }
        
        private ProtoStream ReadStream() {
            try {
                if (!_stream.CanRead) throw new StreamNotReadableException();
                return Serializer.DeserializeWithLengthPrefix<ProtoStream>(_stream, PrefixStyle.Fixed32);
            } catch {
                throw new StreamDecodeException();
            }
        }
        
        private T ReadStream<T>() where T : ProtoStream {
            try {
                if (!_stream.CanRead) throw new StreamNotReadableException();
                var data = Serializer.DeserializeWithLengthPrefix<ProtoStream>(_stream, PrefixStyle.Fixed32);
                try {
                    return (T) data;
                } catch {
                    throw new CommandConversionException();;
                }
            } catch {
                throw new StreamDecodeException();
            }
        }
        
        public void Authenticate(string user, string password) {
            if (_isServer) return;
            Serializer.SerializeWithLengthPrefix(_stream, new RequestAuthentication(user, password), PrefixStyle.Fixed32);
        }     
        
    }
}