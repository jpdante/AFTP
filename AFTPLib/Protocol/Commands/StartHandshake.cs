using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class StartHandshake {
        [ProtoMember(1)] public byte Command = 0;
    }
}