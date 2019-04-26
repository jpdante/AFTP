using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using AFTPLib.Exceptions;
using AFTPLib.Protocol.Args;
using AFTPLib.Protocol.Commands;
using ProtoBuf;

namespace AFTPLib.Protocol {
    public class AftpConnection {

        private Stream _stream;
        private readonly bool _isServer;
        private readonly ConnectionSettings _settings;
        private readonly ConnectionInfo _info;
        private readonly int _defaultTimeout;
        private bool _isAuthenticated;
        private bool _continueReadStream;
        private int _initializationStatus;

        #region Events

        public delegate void CheckVersionHandler(object sender, CheckVersionEventArgs args);
        public CheckVersionHandler OnCheckVersion;

        public delegate Stream RequestSecureStreamHandler(object sender, Stream stream, bool selfSigned);
        public RequestSecureStreamHandler OnRequestSecureStream;

        public delegate void AuthenticateHandler(object sender, AuthenticationRequestEventArgs args);
        public AuthenticateHandler OnAuthenticateRequest;

        public delegate void ErrorHandler(object sender, Exception ex);
        public ErrorHandler OnErrorOccurs;

        #endregion

        public AftpConnection(Stream stream, ConnectionSettings settings = null, bool isServer = false, int timeout = 10) {
            _stream = stream;
            _isServer = isServer;
            _settings = settings;
            _info = new ConnectionInfo();
            if (!_isServer) return;
            _defaultTimeout = timeout;
            _isAuthenticated = false;
            _initializationStatus = 0;
        }

        public void Handshake() {
            //Task.Factory.StartNew(StreamReader);
            if (_isServer) ServerHandshake();
            else ClientHandshake();
        }
        /* Commands by ID
         * 0: StartHandshake
         * 1: GetVersion
         * 2: SetSetting
         * 3: RequestEncryption
         * 4: FinishEncryption
         * 5: FinishHandshake
         */
        /*private void StreamReader() {
            _continueReadStream = true;
            while (_stream.CanRead && _continueReadStream) {
                var protoStream = ReadStream();
                switch (protoStream.CommandId) {
                    case 0:
                        if (_isServer && _initializationStatus == 0) {
                            Serializer.SerializeWithLengthPrefix(_stream, new StartHandshake(), PrefixStyle.Fixed32);
                            _initializationStatus = 1;
                        } else if(_initializationStatus == 0) {
                            Serializer.SerializeWithLengthPrefix(_stream, new GetVersion(_settings.SentVersion, _settings.SentSoftware), PrefixStyle.Fixed32);
                            _initializationStatus = 1;
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 1:
                        var getVersion = (GetVersion)protoStream;
                        _info.Version = getVersion.Version;
                        _info.Software = getVersion.Software;
                        Serializer.SerializeWithLengthPrefix(_stream, new GetVersion(_settings.SentVersion, _settings.SentSoftware), PrefixStyle.Fixed32);
                        break;
                    case 2:
                        var setSetting = (SetSetting)protoStream;
                        if (_isServer && _initializationStatus == 1) {
                            //Future Usage
                        } else if (_initializationStatus == 1) {
                            //Future Usage
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 3:
                        if (!_isServer && _initializationStatus == 1) {
                            _stream = OnRequestSecureStream(this, _stream, _settings.CertificateSelfSigned);
                            Serializer.SerializeWithLengthPrefix(_stream, new FinishEncryption(), PrefixStyle.Fixed32);
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 4:
                        if (_isServer && _initializationStatus == 1) {
                            if (_settings.UseEncryption) {
                                Serializer.SerializeWithLengthPrefix(_stream, new RequestEncryption(_settings.EncryptionType, _settings.CertificateSelfSigned, null), PrefixStyle.Fixed32);
                                _initializationStatus = 2;
                                _stream = OnRequestSecureStream(this, _stream, false);
                            } else {
                                Serializer.SerializeWithLengthPrefix(_stream, new FinishHandshake(), PrefixStyle.Fixed32);
                                _initializationStatus = 3;
                            }
                        } else if (_initializationStatus == 1) {
                            _stream = OnRequestSecureStream(this, _stream, _settings.CertificateSelfSigned);
                            Serializer.SerializeWithLengthPrefix(_stream, new GetVersion(_settings.SentVersion, _settings.SentSoftware), PrefixStyle.Fixed32);
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    case 5:
                        if (_isServer && _initializationStatus == 1) {
                            if (_settings.UseEncryption) {
                                Serializer.SerializeWithLengthPrefix(_stream, new RequestEncryption(_settings.EncryptionType, _settings.CertificateSelfSigned, null), PrefixStyle.Fixed32);
                                _initializationStatus = 2;
                                _stream = OnRequestSecureStream(this, _stream, false);
                            } else {
                                Serializer.SerializeWithLengthPrefix(_stream, new FinishHandshake(), PrefixStyle.Fixed32);
                                _initializationStatus = 3;
                            }
                        } else if (_initializationStatus == 1) {
                            _stream = OnRequestSecureStream(this, _stream, _settings.CertificateSelfSigned);
                            Serializer.SerializeWithLengthPrefix(_stream, new GetVersion(_settings.SentVersion, _settings.SentSoftware), PrefixStyle.Fixed32);
                        } else {
                            HardShutdown(EndConnectionReason.Unknown, new RandomCommandException());
                        }
                        break;
                    default:
                        HardShutdown(EndConnectionReason.InvalidCommand, new UnknownCommandException(protoStream.CommandId));
                        break;
                }
            }
        }*/

        private void HardShutdown(EndConnectionReason reason, Exception error) {
            _continueReadStream = false;
            _isAuthenticated = false;
            if (_stream.CanWrite) Serializer.SerializeWithLengthPrefix(_stream, new EndConnection(reason), PrefixStyle.Fixed32);
            _stream.Close();
            _stream.Dispose();
            OnErrorOccurs(this, error);
        }

        private void ServerHandshake() {
            ReadStream<StartHandshake>();
            Serializer.SerializeWithLengthPrefix(_stream, new StartHandshake(), PrefixStyle.Fixed32);
            var version = ReadStream<GetVersion>();
            _info.Version = version.Version;
            _info.Software = version.Software;
            Serializer.SerializeWithLengthPrefix(_stream, new GetVersion(_settings.SentVersion, _settings.SentSoftware), PrefixStyle.Fixed32);
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
            Serializer.SerializeWithLengthPrefix(_stream, new UserAuthentication(user, password), PrefixStyle.Fixed32);
        }
        
        
    }
}