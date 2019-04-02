using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class GetVersion : ICommand {
        [ProtoMember(1)]
        public byte CommandId => 2;
        [ProtoMember(2)] public int Version;
    }
}