using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class FinishEncryption : ProtoStream {
        public FinishEncryption() : base(4) {
        }
    }
}
