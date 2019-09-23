using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace AFTP.Client.Model.Server {
    public class ServerTreeViewItem {
        public string Name;
        public string Group;
        public readonly bool IsServer;
        public readonly ServerConfig Config;

        public ServerTreeViewItem(string name, bool isServer, ServerConfig config) {
            Name = name;
            IsServer = isServer;
            Config = config;
        }
    }
}
