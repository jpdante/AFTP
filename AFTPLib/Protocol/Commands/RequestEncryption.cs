using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class RequestEncryption : ProtoStream {

        [ProtoMember(2)] public byte Type;
        [ProtoMember(3)] public bool SelfSigned;
        [ProtoMember(3)] public string Data;

        public RequestEncryption() : base(3) {
        }

        public RequestEncryption(EncryptionType type, bool selfSigned, string data) : base(3) {
            Type = (byte)EncryptionType.Ssl;
            SelfSigned = selfSigned;
            Data = data;
        }
    }

    public enum EncryptionType : byte {
        Ssl = 0,
    }
}
