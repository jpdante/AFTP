using AFTPLib.Protocol.Commands.Models;
using ProtoBuf;

namespace AFTPLib.Protocol.Commands.List {
    [ProtoContract]
    public class DirectoryListResponse : ProtoStream {

        [ProtoMember(2)]
        public bool Success;

        [ProtoMember(3)]
        public string ErrorMessage;

        [ProtoMember(4)]
        public DirectoryEntry[] DirectoryEntries;

        [ProtoMember(5)]
        public string Guid;

        public DirectoryListResponse() : base(9) {

        }

        public DirectoryListResponse(bool success, string errorMessage, DirectoryEntry[] directoryEntries, string guid) : base(9) {
            Success = success;
            ErrorMessage = errorMessage;
            DirectoryEntries = directoryEntries;
            Guid = guid;
        }
    }
}