using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class FinishHandshake : ICommand {
        [ProtoMember(1)]
        public byte CommandId => 1;
    }
}