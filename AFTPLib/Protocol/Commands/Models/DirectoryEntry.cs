using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Models {
    [ProtoContract]
    public class DirectoryEntry {

        [ProtoMember(1)]
        public string FileName;

        [ProtoMember(2)]
        public bool IsFile;

        [ProtoMember(3)]
        public long Size;

        [ProtoMember(4)]
        public int Modified;

        [ProtoMember(5)]
        public int Permissions;

        [ProtoMember(6)]
        public string Owner;

        public DirectoryEntry() {
        }

        public DirectoryEntry(string fileName, bool isFile, long size, int modified, int permissions, string owner) {
            FileName = fileName;
            IsFile = isFile;
            Size = size;
            Modified = modified;
            Permissions = permissions;
            Owner = owner;
        }
    }
}