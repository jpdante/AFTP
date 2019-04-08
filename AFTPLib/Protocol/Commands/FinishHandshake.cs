using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class FinishHandshake : ProtoStream {
        public FinishHandshake() : base(5) {
        }
    }
}