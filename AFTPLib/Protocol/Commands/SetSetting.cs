using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class SetSetting : ProtoStream {

        [ProtoMember(2)] public byte Setting;
        [ProtoMember(3)] public string Value;

        public SetSetting() : base(2) {

        }

        public SetSetting(ConnectionSettings setting, string value) : base(2) {
            Setting = (byte)setting;
            Value = value;
        }
    }

    public enum ConnectionSettings : byte {
        SetEncryption = 0,

    }
}
