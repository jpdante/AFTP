using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Other {
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
        UseEncryption = 0,
        EncryptionType = 1,
        SelfSignedCertificate = 2,
        PasswordEncryptionType = 3
    }
}
