using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using AFTP.Client.Enum;
using AFTP.Client.Model.File;
using AFTP.Client.Model.Util;
using AFTP.Client.Model.View;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace AFTP.Client.View.Explorer {
    /// <summary>
    /// Interaction logic for HorizontalServerOnly.xaml
    /// </summary>
    public partial class HorizontalServerOnly : FileExplorer {
        public HorizontalServerOnly() {
            InitializeComponent();
        }

        private string _currentRemotePath;
        private GridViewColumnHeader _listViewSortCol = null;
        private SortAdorner _listViewSortAdorner = null;

        public override event GoToRemotePathHandler OnGoToRemotePath;
        public override event GoToLocalPathHandler OnGoToLocalPath;

        public override void UpdateRemotePath(string path) {
            _currentRemotePath = path;
            this.RemotePathBox.Dispatcher?.Invoke(DispatcherPriority.Normal, new Action(() => {
                var lastChar = path[path.Length - 1];
                if (lastChar != '/' && lastChar != '\\') path += "/";
                RemotePathBox.Text = path;
            }));
        }

        public override void UpdateLocalPath(string path) { }

        public override void UpdateRemoteEntries(RemoteEntry[] remoteEntries) {
            RemoteListView.Items.Clear();
            RemoteListView.Items.Add(new ListViewRemoteEntry());
            foreach (var entry in remoteEntries) {
                RemoteListView.Items.Add(new ListViewRemoteEntry(entry));
            }
        }

        public override void UpdateLocalEntries(LocalEntry[] localEntries) { }

        public void EnterPressRemotePath(string text) { OnGoToRemotePath?.Invoke(this, text); }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e) {
            if (!(((ListViewItem)sender).Content is ListViewRemoteEntry listViewRemoteEntry)) return;
            if (listViewRemoteEntry.IsBack) {
                OnGoToRemotePath?.Invoke(this, GetParentDirPath(_currentRemotePath));
            } else {
                if (listViewRemoteEntry.RemoteEntry.Type == RemoteEntryType.Directory) {
                    OnGoToRemotePath?.Invoke(this, listViewRemoteEntry.RemoteEntry.FullName);
                }
            }
        }

        private void RemotePathBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != Key.Return) return;
            OnGoToRemotePath?.Invoke(this, RemotePathBox.Text);
        }

        public string GetParentDirPath(string path) {
            var index = path.Trim('/', '\\').LastIndexOfAny(new char[] { '\\', '/' }) + 1;
            if (index == 0) return "/";
            return index >= 0 ? path.Remove(index) : "";
        }

        private void RemoteListColumnHeader_Click(object sender, RoutedEventArgs e) {
            var column = (sender as GridViewColumnHeader);
            var sortBy = column?.Tag.ToString();
            if (_listViewSortCol != null) {
                AdornerLayer.GetAdornerLayer(_listViewSortCol)?.Remove(_listViewSortAdorner);
                RemoteListView.Items.SortDescriptions.Clear();
            }

            var newDir = ListSortDirection.Ascending;
            if (_listViewSortCol == column && _listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            _listViewSortCol = column;
            _listViewSortAdorner = new SortAdorner(_listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(_listViewSortCol)?.Add(_listViewSortAdorner);
            RemoteListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }
}