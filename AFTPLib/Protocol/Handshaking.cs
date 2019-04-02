using System;
using System.IO;
using System.Threading.Tasks;
using AFTPLib.Exceptions;
using AFTPLib.Protocol.Args;
using AFTPLib.Protocol.Commands;
using ProtoBuf;

namespace AFTPLib.Protocol
{
    public class Handshaking {
        private Stream _stream;
        private byte[] _buffer;
        private bool _readStream;
        private byte _handshakeProgress;
        private int _version;
        private byte _systemType;
        private string _systemName;
        
        public delegate void HandshakeFinishHandler(object sender, HandshakeFinishEventArgs args);
        public HandshakeFinishHandler OnHandshakeFinish;
        
        public delegate void HandshakeReceiveVersionHandler(object sender, HandshakeReceiveDataEventArgs args);
        public HandshakeReceiveVersionHandler OnHandshakeReceiveData;

        public Handshaking(Stream stream) {
            _stream = stream;
            _buffer = new byte[2048];
        }

        public void StartHandshake() {
            //_stream.BeginRead(_buffer, 0, _buffer.Length, BeginReadCallback, null);
            _handshakeProgress = 0;
            _readStream = true;
            Task.Factory.StartNew(ReadLoop);
            Serializer.SerializeWithLengthPrefix(_stream, new ProtoStream(new StartHandshake()), PrefixStyle.Fixed32);
        }

        private void ReadLoop() {
            while (_readStream && _stream.CanRead) {
                ProtoStream command = Serializer.DeserializeWithLengthPrefix<ProtoStream>(_stream, PrefixStyle.Fixed32);
                switch (command.CommandId) {
                    case 0:
                        if (_handshakeProgress == 0) {
                            _handshakeProgress = 1;
                            Serializer.SerializeWithLengthPrefix(_stream, new ProtoStream(new GetVersion()), PrefixStyle.Fixed32);
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        }
                        break;
                    case 1:
                        if (_handshakeProgress == 3) {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(true, null));
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        }
                        break;
                    case 2:
                        if (_handshakeProgress == 1) {
                            _handshakeProgress = 2;
                            _version = ((GetVersion) command.Command).Version;
                            Serializer.SerializeWithLengthPrefix(_stream, new ProtoStream(new GetSystem()), PrefixStyle.Fixed32);
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        }
                        break;
                    case 3:
                        if (_handshakeProgress == 2) {
                            _systemType = ((GetSystem) command.Command).SystemType;
                            _systemName = ((GetSystem) command.Command).SystemName;
                            var eventArgs = new HandshakeReceiveDataEventArgs(_version, _systemType, _systemName);
                            OnHandshakeReceiveData(this, eventArgs);
                            if (eventArgs.Cancel) {
                                switch (eventArgs.CancelReason) {
                                    case HandshakeCancelReason.UnsupportedVersion:
                                        _readStream = false;
                                        Serializer.SerializeWithLengthPrefix(_stream, new ProtoStream(new HandshakeError("Unsupported protocol version.")), PrefixStyle.Fixed32);
                                        OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeCanceledException(HandshakeCancelReason.UnsupportedVersion)));
                                        break;
                                    case HandshakeCancelReason.UnsupportedSystem:
                                        _readStream = false;
                                        Serializer.SerializeWithLengthPrefix(_stream, new ProtoStream(new HandshakeError("Unsupported software or system.")), PrefixStyle.Fixed32);
                                        OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeCanceledException(HandshakeCancelReason.UnsupportedSystem)));
                                        break;
                                    case HandshakeCancelReason.Unknown:
                                        _readStream = false;
                                        Serializer.SerializeWithLengthPrefix(_stream, new ProtoStream(new HandshakeError("Unknown protocol error.")), PrefixStyle.Fixed32);
                                        OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeCanceledException(HandshakeCancelReason.Unknown)));
                                        break;
                                }
                            } else {
                                _handshakeProgress = 3;
                                Serializer.SerializeWithLengthPrefix(_stream, new ProtoStream(new FinishHandshake()), PrefixStyle.Fixed32);
                            }
                        } else {
                            _readStream = false;
                            OnHandshakeFinish(this, new HandshakeFinishEventArgs(false, new HandshakeErrorException()));
                        }
                        break;
                    default:
                        _readStream = false;
                        break;
                }
            }
        }
        
        /*private void BeginReadCallback(IAsyncResult ar) {
            int bytesRead = _stream.EndRead(ar);
            Serializer.DeserializeWithLengthPrefix<ProtoStream>(_stream, PrefixStyle.Fixed32);
            _stream.BeginRead(_buffer, 0, _buffer.Length, BeginReadCallback, null);
        }*/
    }
}