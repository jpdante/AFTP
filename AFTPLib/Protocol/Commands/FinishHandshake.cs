using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class FinishHandshake {
        [ProtoMember(1)] public byte Command = 2;
    }
}