using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class GetVersion {
        [ProtoMember(1)] public byte Command = 2;
        [ProtoMember(2)] public string Version;
    }
}