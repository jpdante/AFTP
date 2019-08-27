using System;
using System.Collections.Generic;
using AFTP.Client.Enum;

namespace AFTP.Client.Model.Server {
    public class ServerConfig {
        public ServerType Type;
        public string Name;
        public string Group;
        public string Host;
        public int Port;
        public Dictionary<ServerSettingsType, string> Settings;
        public string Uid;

        public ServerConfig() {
            Uid = Guid.NewGuid().ToString();
            Settings = new Dictionary<ServerSettingsType, string>();
        }
    }
}
