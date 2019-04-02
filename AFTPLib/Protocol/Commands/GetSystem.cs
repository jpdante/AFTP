using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class GetSystem {
        [ProtoMember(1)] public byte Command = 3;
        [ProtoMember(2)] public byte SystemType;
        [ProtoMember(3)] public string SystemName;
    }
}