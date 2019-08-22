using System;
using System.Collections.Generic;
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
using AFTP.Client.Model;
using MahApps.Metro.Controls;

namespace AFTP.Client.Window {
    /// <summary>
    /// Interaction logic for ConnectionManager.xaml
    /// </summary>
    public partial class ServerManager : MetroWindow {

        public List<ServerSettings> ServerSettings;
        public int CurrentEdit = -1;
        public bool AllowEvents = false;

        public ServerManager(List<ServerSettings> serverSettings) {
            ServerSettings = serverSettings;
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void NewServer_Click(object sender, RoutedEventArgs e) {
            ServerSettings.Add(new ServerSettings() {
                Name = GenerateServerName(),
            });
            UpdateList();
        }

        private void UpdateList() {
            ServerTreeView.Items.Clear();
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
        }

        private void ServerTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (!(ServerTreeView.SelectedItem is TreeViewItem item)) return;
            AllowEvents = false;
            CurrentEdit = -1;
            foreach (var server in ServerSettings) {
                if (!server.Uid.Equals(item.Uid, StringComparison.CurrentCultureIgnoreCase)) continue;
                CurrentEdit = ServerSettings.IndexOf(server);
                ServerName.Text = server.Name;
                ServerGroup.Text = server.Group;
                SetInputEnabled(true);
                break;
            }
            if (CurrentEdit == -1) SetInputEnabled(false);
            AllowEvents = true;
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

        }
    }
}
