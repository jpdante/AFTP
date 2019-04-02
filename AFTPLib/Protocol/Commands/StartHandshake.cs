using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class StartHandshake : ICommand {
        [ProtoMember(1)]
        public byte CommandId => 0;
    }
}