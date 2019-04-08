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
    public class Handshaking {
        private readonly Stream _stream;
        private readonly int _version;
        private readonly string _software;
        private readonly System.Timers.Timer _timer;
        private readonly bool _isServer;

        private byte[] _buffer;
        private bool _readStream;
        private byte _handshakeProgress;
        private int _timeout;

        public delegate void HandshakeFinishHandler(object sender, HandshakeFinishEventArgs args);
        public HandshakeFinishHandler OnHandshakeFinish;
        
        public delegate void HandshakeReceiveVersionHandler(object sender, HandshakeReceiveDataEventArgs args);
        public HandshakeReceiveVersionHandler OnHandshakeReceiveData;

        public Handshaking(Stream stream, bool isServer, int timeout, int version, string software) {
            _stream = stream;
            _buffer = new byte[2048];
            _isServer = isServer;
            _timeout = timeout;
            _version = version;
            _software = software;
            if (!isServer) return;
            _timer = new System.Timers.Timer { Interval = 1000 };
            _timer.Elapsed += (sender, args) => {
                _timeout--;
                if (timeout > 0) return;
                _timer.Stop();
                _readStream = false;
                OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeTimeoutException()));
            };
        }

        public void StartHandshake() {
            _handshakeProgress = 0;
            _readStream = true;
            Task.Factory.StartNew(ReadLoop);
            if (!_isServer) return;
            _timer.Start();
            SendCommand(new StartHandshake());
        }

        public void SendCommand(ProtoStream protoStream) => Serializer.SerializeWithLengthPrefix(_stream, protoStream, PrefixStyle.Fixed32);

        private void ReadLoop() {
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
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new UnknownCommandException()));
                        }
                        break;
                    case 1:
                        if (_handshakeProgress == 1 && !_isServer) {
                            GetVersion getVersion = (GetVersion) command;
                            var eventArgs = new HandshakeReceiveDataEventArgs(false, HandshakeCancelReason.Unknown, getVersion.Version, getVersion.Software);
                            OnHandshakeReceiveData(this, eventArgs);
                            if (eventArgs.Cancel) {
                                _readStream = false;
                                OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new RandomCommandException(eventArgs.CancelReason)));
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
                                OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new RandomCommandException(eventArgs.CancelReason)));
                                break;
                            }
                            SendCommand(new FinishHandshake());
                            _handshakeProgress = 2;
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new UnknownCommandException()));
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
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new UnknownCommandException()));
                        }
                        break;
                    default:
                        _readStream = false;
                        OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new UnknownCommandException()));
                        break;
                }
            }
        }
    }
}