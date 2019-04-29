using System;
using System.IO;
using AFTPLib.Exceptions;
using AFTPLib.Protocol.Args;
using AFTPLib.Protocol.Commands;
using ProtoBuf;

namespace AFTPLib.Protocol {
    public class AftpConnection {

        private readonly Stream _stream;
        private readonly bool _isServer;
        private readonly ConnectionSettings _settings;
        private readonly ConnectionInfo _info;
        private readonly int _defaultTimeout;

        #region Events

        public delegate void CheckVersionHandler(object sender, CheckVersionEventArgs args);
        public CheckVersionHandler OnCheckVersion;

        #endregion
        
        public AftpConnection(Stream stream, ConnectionSettings settings = null, bool isServer = false, int timeout = 10) {
            _stream = stream;
            _isServer = isServer;
            _settings = settings;
            _info = new ConnectionInfo();
            if (!_isServer) return;
            _defaultTimeout = timeout;
        }

        public void Handshake() {
            if(_isServer) ServerHandshake();
            else ClientHandshake();
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
        
        public void Authenticate() {
            
        }
        
        
    }
}