using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Threading;
using AFTP.Client.Enum;
using AFTP.Client.Model.File;
using AFTP.Client.Model.Util;
using AFTP.Client.Model.View;
using AFTP.Client.Window;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AFTP.Client.View.Explorer {
    /// <summary>
    /// Interaction logic for HorizontalServerOnly.xaml
    /// </summary>
    public partial class HorizontalServerOnly : FileExplorer {

        public override event GoToRemotePathHandler OnGoToRemotePath;
        public override event GoToLocalPathHandler OnGoToLocalPath;

        public override string CurrentRemotePath { get; set; }
        private GridViewColumnHeader _listViewSortCol = null;
        private SortAdorner _listViewSortAdorner = null;
        private readonly TreeViewEntry _defaultTreeViewItem;
        private bool _allowSelectEvent = true;

        public HorizontalServerOnly() {
            InitializeComponent();
            _defaultTreeViewItem = new TreeViewEntry("/", "");
            RemoteTreeView.Items.Add(_defaultTreeViewItem);
        }

        public override void UpdateRemotePath(string path) {
            CurrentRemotePath = path;
            this.RemotePathBox.Dispatcher?.Invoke(DispatcherPriority.Normal, new Action(() => {
                RemotePathBox.Text = path;
            }));
        }

        public void LoadDirectories(string path, TreeViewEntry treeViewItem) {
            if (path[0] == '/') path = path.Remove(0, 1);
            var data = path.Split(new[] { '/' }, 2);
            var subItem = treeViewItem.Children.Cast<TreeViewEntry>().FirstOrDefault(item => item.Directory.Equals(data[0]));
            if (subItem == null) {
                subItem = new TreeViewEntry(data[0], treeViewItem.Path + "/" + data[0]);
                treeViewItem.Children.Add(subItem);
            }
            if (data.Length <= 1) return;
            LoadDirectories(data[1], subItem);
        }

        public void SelectDirectory(string path, TreeViewEntry treeViewItem) {
            if (path[0] == '/') path = path.Remove(0, 1);
            var data = path.Split(new[] { '/' }, 2);
            TreeViewEntry subItem = null;
            foreach (var item in treeViewItem.Children) {
                if (item.Directory.Equals(data[0])) subItem = item;
            }
            if (subItem == null) {
                return;
            }
            if (data.Length <= 1) {
                subItem.IsNodeSelected = true;
                return;
            }
            SelectDirectory(data[1], subItem);
        }

        public override void UpdateLocalPath(string path) { }

        public override void UpdateRemoteEntries(RemoteEntry[] remoteEntries) {
            this.Dispatcher?.Invoke(DispatcherPriority.Normal, new Action(() => {
                RemoteListView.Items.Clear();
                RemoteListView.Items.Add(new ListViewRemoteEntry());
                foreach (var entry in remoteEntries) {
                    RemoteListView.Items.Add(new ListViewRemoteEntry(entry));
                    if (entry.Type != RemoteEntryType.Directory) continue;
                    LoadDirectories(entry.FullName, _defaultTreeViewItem);
                }
                _allowSelectEvent = false;
                SelectDirectory(CurrentRemotePath, _defaultTreeViewItem);
                _allowSelectEvent = true;
            }));
        }

        public override void UpdateLocalEntries(LocalEntry[] localEntries) { }

        public void EnterPressRemotePath(string text) { OnGoToRemotePath?.Invoke(this, text); }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e) {
            if (!(((ListViewItem)sender).Content is ListViewRemoteEntry listViewRemoteEntry)) return;
            if (listViewRemoteEntry.IsBack) {
                OnGoToRemotePath?.Invoke(this, GetParentDirPath(CurrentRemotePath));
            } else {
                if (listViewRemoteEntry.RemoteEntry.Type == RemoteEntryType.Directory) {
                    RemoteListView.Items.Clear();
                    CollectionViewSource.GetDefaultView(RemoteListView.Items).Refresh();
                    OnGoToRemotePath?.Invoke(this, listViewRemoteEntry.RemoteEntry.FullName);
                }
            }
        }

        private void RemotePathBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key != Key.Return) return;
            RemoteListView.Items.Clear();
            CollectionViewSource.GetDefaultView(RemoteListView.Items).Refresh();
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

        private void RemoteTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (!_allowSelectEvent) return;
            if (!(RemoteTreeView.SelectedItem is TreeViewEntry item)) return;
            RemoteListView.Items.Clear();
            CollectionViewSource.GetDefaultView(RemoteListView.Items).Refresh();
            if (string.IsNullOrEmpty(item.Path)) OnGoToRemotePath?.Invoke(this, "/");
            else OnGoToRemotePath?.Invoke(this, item.Path);
        }
    }
}