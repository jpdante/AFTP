using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using AFTP.Client.Enum;

namespace AFTP.Client.Model {
    public class ServerSettings {
        public ServerType ServerType;
        public string Name;
        public string Group;
        public string Host;
        public int Port;
        public Dictionary<ServerSettingsType, string> Settings;
        public string Uid;

        public ServerSettings() {
            Uid = Guid.NewGuid().ToString();
        }
    }
}
