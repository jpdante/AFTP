using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AFTPLib.Exceptions;
using AFTPLib.Protocol.Args;
using AFTPLib.Protocol.Commands;
using ProtoBuf;

namespace AFTPLib.Protocol
{
    public class Authentication {
        private readonly Stream _stream;
        private readonly bool _isServer;

        private byte[] _buffer;

        public delegate void CheckAuthenticationHandler(object sender, HandshakeFinishEventArgs args);
        public CheckAuthenticationHandler OnCheckAuthentication;

        public Authentication(Stream stream, bool isServer) {
            _stream = stream;
            _buffer = new byte[2048];
            _isServer = isServer;
        }

        public void StartAuthentication(string user = null, string password = null) {
            //_handshakeProgress = 0;
            //_readStream = true;
            //Task.Factory.StartNew(ReadLoop);
            if (_isServer) {
                var command = Serializer.DeserializeWithLengthPrefix<ProtoStream>(_stream, PrefixStyle.Fixed32);
                Console.WriteLine(command.ToString());
            } else {
                SendCommand(new RequestAuthentication(user, password));
            }

            //SendCommand(new StartHandshake());
        }

        private void SendCommand(ProtoStream protoStream) => Serializer.SerializeWithLengthPrefix(_stream, protoStream, PrefixStyle.Fixed32);
        
        /*private void ReadLoop() {
            while (_readStream && _stream.CanRead) {
                ProtoStream command = Serializer.DeserializeWithLengthPrefix<ProtoStream>(_stream, PrefixStyle.Fixed32);
                switch (command.CommandId) {
                    case 0:
                        if (_handshakeProgress == 0 && !_isServer) {
                            SendCommand(new StartHandshake());
                            _handshakeProgress = 1;
                        } else if (_handshakeProgress == 0 && _isServer) {
                            SendCommand(new GetVersion(_version, _software));
                            _handshakeProgress = 1;
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        }
                        break;
                    case 1:
                        if (_handshakeProgress == 1 && !_isServer) {
                            GetVersion getVersion = (GetVersion) command;
                            var eventArgs = new HandshakeReceiveDataEventArgs(false, HandshakeCancelReason.Unknown, getVersion.Version, getVersion.Software);
                            OnHandshakeReceiveData(this, eventArgs);
                            if (eventArgs.Cancel) {
                                _readStream = false;
                                OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeCanceledException(eventArgs.CancelReason)));
                                break;
                            }
                            SendCommand(new GetVersion(_version, _software));
                            _handshakeProgress = 2;
                        } else if (_handshakeProgress == 1 && _isServer) {
                            GetVersion getVersion = (GetVersion)command;
                            var eventArgs = new HandshakeReceiveDataEventArgs(false, HandshakeCancelReason.Unknown, getVersion.Version, getVersion.Software);
                            OnHandshakeReceiveData(this, eventArgs);
                            if (eventArgs.Cancel) {
                                _readStream = false;
                                OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeCanceledException(eventArgs.CancelReason)));
                                break;
                            }
                            SendCommand(new FinishHandshake());
                            _handshakeProgress = 2;
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        }
                        break;
                    case 2:
                        if (_handshakeProgress == 2 && !_isServer) {
                            _readStream = false;
                            SendCommand(new FinishHandshake());
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(true, null));
                        } else if (_handshakeProgress == 2 && _isServer) {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(true, null));
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        }
                        break;
                    default:
                        _readStream = false;
                        OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        break;
                }
            }*/
        //}
    }
}