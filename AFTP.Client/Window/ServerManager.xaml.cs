using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AFTP.Client.Enum;
using AFTP.Client.Model;
using AFTP.Client.Model.Server;
using AFTP.Client.View.ProtocolConfig;
using MahApps.Metro.Controls;

namespace AFTP.Client.Window {
    /// <summary>
    /// Interaction logic for ConnectionManager.xaml
    /// </summary>
    public partial class ServerManager : MetroWindow {

        public List<ServerConfig> ServerSettings;
        public bool Success { get; private set; }
        public int CurrentEdit = -1;
        public bool AllowEvents = false;

        public ObservableCollection<ServerTreeViewItem> TreeViewData;

        public ServerManager(IReadOnlyCollection<ServerConfig> serverSettings) {
            InitializeComponent();
            ServerSettings = new List<ServerConfig>(serverSettings.Count);
            TreeViewData = new ObservableCollection<ServerTreeViewItem>();
            ServerTreeView.TreeView.ItemsSource = TreeViewData;
            foreach (var server in serverSettings) {
                ServerSettings.Add(server);
                TreeViewData.Add(new ServerTreeViewItem(server.Name, true, server));
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e) {
            Success = true;
            this.Close();
        }

        private void NewServer_Click(object sender, RoutedEventArgs e) {
            ServerSettings.Add(new ServerConfig() {
                Name = GenerateServerName(),
            });
            UpdateList();
        }

        private void UpdateList() {
            /*ServerTreeView.Items.Clear();
            foreach (var server in ServerSettings) {
                if (string.IsNullOrEmpty(server.Group)) {
                    ServerTreeView.Items.Add(new TreeViewItem() {
                        Header = server.Name,
                        Uid = server.Uid
                    });
                    continue;
                }
                var hasGroup = false;
                foreach (TreeViewItem treeViewItem in ServerTreeView.Items) {
                    if (!treeViewItem.Header.Equals(server.Group)) continue;
                    treeViewItem.Items.Add(new TreeViewItem() {
                        Header = server.Name,
                        Uid = server.Uid
                    });
                    hasGroup = true;
                }
                if (hasGroup) continue;
                var groupItem = new TreeViewItem() {
                    Header = server.Group,
                    IsExpanded = true
                };
                groupItem.Items.Add(new TreeViewItem() {
                    Header = server.Name,
                    Uid = server.Uid
                });
                ServerTreeView.Items.Add(groupItem);
            }
            if (CurrentEdit == -1) return;
            foreach (TreeViewItem treeViewItem in ServerTreeView.Items) {
                foreach (TreeViewItem treeViewItem2 in treeViewItem.Items) {
                    if (!treeViewItem2.Uid.Equals(ServerSettings[CurrentEdit].Uid)) continue;
                    treeViewItem2.IsSelected = true;
                    return;
                }
                if (!treeViewItem.Uid.Equals(ServerSettings[CurrentEdit].Uid)) continue;
                treeViewItem.IsSelected = true;
                return;
            }*/
        }

        private string GenerateServerName() {
            var id = 0;
            foreach (var server in ServerSettings) {
                if(server.Name.Equals($"Server{id}", StringComparison.CurrentCultureIgnoreCase)) id++;
            }
            return $"Server{id}";
        }

        private void SetInputEnabled(bool isEnabled) {
            ServerName.IsEnabled = isEnabled;
            ServerGroup.IsEnabled = isEnabled;
            ServerProtocol.IsEnabled = isEnabled;
            RenameServer.IsEnabled = isEnabled;
            DeleteServer.IsEnabled = isEnabled;
            DuplicateServer.IsEnabled = isEnabled;
            ConnectServer.IsEnabled = isEnabled;
            /*LocalDirectory.IsEnabled = isEnabled;
            RemoteDirectory.IsEnabled = isEnabled;
            BrowserLocalDirectory.IsEnabled = isEnabled;
            TransferModeActive.IsEnabled = isEnabled;
            TranferModeDefault.IsEnabled = isEnabled;
            TransferModePassive.IsEnabled = isEnabled;
            LimitSimultaneousConnections.IsEnabled = isEnabled;
            CharsetEncodingAutoDetect.IsEnabled = isEnabled;
            CharsetEncodingASCII.IsEnabled = isEnabled;
            CharsetEncodingUTF8.IsEnabled = isEnabled;*/
        }

        private void ServerTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
           /* if (!(ServerTreeView.SelectedItem is TreeViewItem item)) return;
            AllowEvents = false;
            CurrentEdit = -1;
            foreach (var server in ServerSettings) {
                if (!server.Uid.Equals(item.Uid, StringComparison.CurrentCultureIgnoreCase)) continue;
                CurrentEdit = ServerSettings.IndexOf(server);
                ServerName.Text = server.Name;
                ServerGroup.Text = server.Group;
                switch (server.Type) {
                    case ServerType.Aftp:
                        ProtocolConfigFrame.Content = new AftpConfig(server);
                        break;
                    case ServerType.Ftp:
                        ProtocolConfigFrame.Content = new FtpConfig();
                        break;
                    case ServerType.Sftp:
                        ProtocolConfigFrame.Content = new SftpConfig();
                        break;
                    case ServerType.S3:
                        ProtocolConfigFrame.Content = new S3Config();
                        break;
                    case ServerType.Smb:
                        ProtocolConfigFrame.Content = new SmbConfig();
                        break;
                    default:
                        break;
                }

                SetInputEnabled(true);
                break;
            }
            if (CurrentEdit == -1) {
                SetInputEnabled(false);
                ProtocolConfigFrame.Content = null;
            }
            AllowEvents = true;*/
        }

        private void ServerName_TextChanged(object sender, TextChangedEventArgs e) {
            if (!AllowEvents) return;
            ServerSettings[CurrentEdit].Name = ServerName.Text;
            UpdateList();
        }

        private void ServerGroup_TextChanged(object sender, TextChangedEventArgs e) {
            if (!AllowEvents) return;
            ServerSettings[CurrentEdit].Group = ServerGroup.Text;
            UpdateList();
        }

        private void ServerProtocol_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!AllowEvents) return;
            switch (ServerProtocol.SelectedIndex) {
                case 0:
                    ServerSettings[CurrentEdit].Type = ServerType.Aftp;
                    ProtocolConfigFrame.Content = new AftpConfig(ServerSettings[CurrentEdit]);
                    break;
                case 1:
                    ServerSettings[CurrentEdit].Type = ServerType.Ftp;
                    ProtocolConfigFrame.Content = new FtpConfig();
                    break;
                case 2:
                    ServerSettings[CurrentEdit].Type = ServerType.Sftp;
                    ProtocolConfigFrame.Content = new SftpConfig();
                    break;
                case 3:
                    ServerSettings[CurrentEdit].Type = ServerType.S3;
                    ProtocolConfigFrame.Content = new S3Config();
                    break;
                case 4:
                    ServerSettings[CurrentEdit].Type = ServerType.Smb;
                    ProtocolConfigFrame.Content = new SmbConfig();
                    break;
                default:
                    break;
            }
        }

        private void RenameServer_Click(object sender, RoutedEventArgs e) {

        }

        private void DeleteServer_Click(object sender, RoutedEventArgs e) {
            ServerSettings.RemoveAt(CurrentEdit);
            CurrentEdit = -1;
            ProtocolConfigFrame.Content = null;
            SetInputEnabled(false);
            UpdateList();
        }

        private void DuplicateServer_Click(object sender, RoutedEventArgs e) {
            var server = ServerSettings[CurrentEdit];
            server.Name += "-Copy";
            ServerSettings.Add(server);
            CurrentEdit = -1;
            ProtocolConfigFrame.Content = null;
            SetInputEnabled(false);
            UpdateList();
        }

        private void ConnectServer_Click(object sender, RoutedEventArgs e) {

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
            UpdateList();
        }
    }
}
