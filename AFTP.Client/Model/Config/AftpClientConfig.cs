using System.Collections.Generic;

namespace AFTP.Client.Model.Config {
    public class AftpClientConfig {
        public string ConfigVersion;
        public List<ServerConfig> Servers;

        public AftpClientConfig() {
            ConfigVersion = "1";
            Servers = new List<ServerConfig>();
        }
    }
}
