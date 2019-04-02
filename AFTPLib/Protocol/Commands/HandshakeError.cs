using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class HandshakeError : ICommand {
        [ProtoMember(1)]
        public byte CommandId => 255;
        [ProtoMember(2)] public string Reason;

        public HandshakeError(string reason) {
            Reason = reason;
        }
    }
}