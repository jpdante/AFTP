using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    [ProtoInclude(100, typeof(StartHandshake))]
    [ProtoInclude(101, typeof(FinishHandshake))]
    [ProtoInclude(102, typeof(GetVersion))]
    [ProtoInclude(103, typeof(RequestAuthentication))]
    [ProtoInclude(104, typeof(SetSetting))]
    [ProtoInclude(105, typeof(AuthenticationResponse))]
    [ProtoInclude(106, typeof(RequestEncryption))]
    [ProtoInclude(107, typeof(FinishEncryption))]
    [ProtoInclude(108, typeof(EndConnection))]
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
