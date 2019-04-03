using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace AFTPLib.Configuration {
    public class ServerConfig {
        public IPEndPoint IpEndPoint;
        public int BackLog = 10;
        public X509Certificate ServerCertificate;
        public int FirewallMaxConnections = 10;
        public int FirewallMaxFailedAttempts = 3;
        public int FirewallBanTimeout = 10;
        public int FirewallHistoryTimeout = 3600;
        public int HandshakeTimeout = 5;
        public int Version = 1;
        public string Software = "AFTPLib";
    }
}