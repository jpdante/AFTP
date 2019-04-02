using AFTPLib.Protocol.Commands;
using ProtoBuf;

namespace AFTPLib.Protocol {
    [ProtoContract]
    public class ProtoStream {
        [ProtoMember(1)] public byte CommandId;
        [ProtoMember(1)] public object Command;
        public ProtoStream(ICommand command) {
            CommandId = command.CommandId;
            Command = command;
        }
    }
}