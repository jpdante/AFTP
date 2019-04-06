using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    [ProtoInclude(10, typeof(StartHandshake))]
    [ProtoInclude(11, typeof(FinishHandshake))]
    [ProtoInclude(12, typeof(GetVersion))]
    [ProtoInclude(13, typeof(UserAuthentication))]
    public class ProtoStream {
        [ProtoMember(1)]
        public byte CommandId;

        public ProtoStream(byte id) {
            CommandId = id;
        }

        public override string ToString() {
            return $"CommandId: {CommandId}";
        }
    }
}
