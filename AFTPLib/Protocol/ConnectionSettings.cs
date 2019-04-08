using AFTPLib.Protocol.Commands;

namespace AFTPLib.Protocol {
    public class ConnectionSettings {
        
        public int SentVersion;
        public string SentSoftware = "AFTPLib";
        public bool UseEncryption = true;
        public EncryptionType EncryptionType = EncryptionType.Ssl;
        public bool CertificateSelfSigned = false;

    }
}