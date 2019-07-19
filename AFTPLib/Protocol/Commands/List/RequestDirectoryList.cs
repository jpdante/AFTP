using ProtoBuf;

namespace AFTPLib.Protocol.Commands.List {
    [ProtoContract]
    public class RequestDirectoryList : ProtoStream {

        [ProtoMember(2)]
        public string Directory;
        [ProtoMember(3)]
        public string Guid;

        public RequestDirectoryList() : base(8) {

        }

        public RequestDirectoryList(string directory, string guid) : base(8) {
            Directory = directory;
            Guid = guid;
        }
    }
}