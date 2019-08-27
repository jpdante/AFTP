using System.Collections.ObjectModel;

namespace AFTP.Client.Model.Server {
    public class ServerTreeViewItem : NotifyPropertyChanged {
        public readonly bool IsServer;
        public readonly ServerConfig Config;
        public ObservableCollection<ServerTreeViewItem> TreeViewChildrenItems { get; set; }

        private string _name;
        public string Name {
            get => _name;
            set => SetField(ref _name, value);
        }

        public ServerTreeViewItem(string name, bool isServer, ServerConfig config) {
            Name = name;
            IsServer = isServer;
            Config = config;
            TreeViewChildrenItems = new ObservableCollection<ServerTreeViewItem>();
        }
    }
}
