using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Other {
    [ProtoContract]
    public class GetVersion : ProtoStream {

        [ProtoMember(2)] public int Version;
        [ProtoMember(3)] public string Software;

        public GetVersion() : base(1) {

        }

        public GetVersion(int version, string software) : base(1) {
            Version = version;
            Software = software;
        }
    }
}
