using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class GetSystem : ICommand {
        [ProtoMember(1)]
        public byte CommandId => 3;
        [ProtoMember(2)] public byte SystemType;
        [ProtoMember(3)] public string SystemName;
    }
}