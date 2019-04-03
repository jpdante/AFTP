using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class StartHandshake : ProtoStream {
        public StartHandshake() : base(0) {
        }
    }
}