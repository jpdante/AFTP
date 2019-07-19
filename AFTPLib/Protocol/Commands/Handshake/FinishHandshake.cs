using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Handshake {
    [ProtoContract]
    public class FinishHandshake : ProtoStream {
        public FinishHandshake() : base(5) {
        }
    }
}