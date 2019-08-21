using AFTPLib.Protocol.Commands;

namespace AFTPLib.Protocol {
    public class ConnectionSettings {
        
        public int SentVersion;
        public string SentSoftware = "AFTPLib";
        public bool UseEncryption = true;
        public ConnectionEncryptionType ConnectionEncryptionType = ConnectionEncryptionType.Ssl;
        public PasswordEncryptionType PasswordEncryptionType = PasswordEncryptionType.Sha256;
        public bool CertificateSelfSigned = false;

    }

    public enum ConnectionEncryptionType : byte {
        Ssl = 1,
    }

    public enum PasswordEncryptionType {
        PlainText,
        Md5,
        Sha1,
        Sha256,
        Sha384,
        Sha512
    }
}