using System.Net;

namespace AFTPLib.Configuration {
    public class ServerConfig {
        public IPEndPoint Address { get; set; }
        public int BackLog { get; set; }
        
    }
}