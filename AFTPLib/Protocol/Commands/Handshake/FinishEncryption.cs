using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Handshake {
    [ProtoContract]
    public class FinishEncryption : ProtoStream {
        public FinishEncryption() : base(4) {
        }
    }
}
