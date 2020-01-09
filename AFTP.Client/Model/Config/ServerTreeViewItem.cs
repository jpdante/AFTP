namespace AFTP.Client.Model.Config {
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
