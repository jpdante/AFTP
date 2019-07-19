using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Handshake {
    [ProtoContract]
    public class StartHandshake : ProtoStream {
        public StartHandshake() : base(0) {
        }
    }
}