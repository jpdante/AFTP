using System;
using System.Collections.Generic;
using System.Text;
using AFTPLib.Protocol.Commands.Handshake;
using AFTPLib.Protocol.Commands.List;
using AFTPLib.Protocol.Commands.Models;
using AFTPLib.Protocol.Commands.Other;
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
    [ProtoInclude(109, typeof(RequestDirectoryList))]
    [ProtoInclude(110, typeof(DirectoryListResponse))]
    [ProtoInclude(111, typeof(DirectoryEntry))]

    public class ProtoStream {

        /*  Commands by ID
         * 00: SR StartHandshake
         * 01: SR GetVersion
         * 02: SR SetSetting
         * 03: S- RequestEncryption
         * 04: -R FinishEncryption
         * 05: SR FinishHandshake
         * 06: S- RequestAuthentication
         * 07: -R AuthenticationResponse
         * 08: S- RequestPathList
         * 09: -R PathListResponse
         * 10: SR 
         * 11: SR 
         * 12: SR 
         * 13: SR 
         * 14: SR
         * 15: SR
         */

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
