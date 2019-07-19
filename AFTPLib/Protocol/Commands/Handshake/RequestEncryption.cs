using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Handshake {
    [ProtoContract]
    public class RequestEncryption : ProtoStream {

        [ProtoMember(2)] public byte Type;
        [ProtoMember(3)] public bool SelfSigned;
        [ProtoMember(4)] public string Data;

        public RequestEncryption() : base(3) {
        }

        public RequestEncryption(ConnectionEncryptionType type, bool selfSigned, string data) : base(3) {
            Type = (byte)ConnectionEncryptionType.Ssl;
            SelfSigned = selfSigned;
            Data = data;
        }
    }
}
