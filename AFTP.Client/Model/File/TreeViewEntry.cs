using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace AFTP.Client.Model.File {
    public class TreeViewEntry : INotifyPropertyChanged  {

        public string Path { get; set; }
        public string Directory { get; set; }
        public Bitmap Icon { get; set; }

        private bool _isNodeExpanded;
        private bool _isNodeSelected;

        public bool IsNodeExpanded {
            get => _isNodeExpanded;
            set {
                _isNodeExpanded = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsNodeSelected {
            get => _isNodeSelected;
            set {
                _isNodeSelected = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<TreeViewEntry> Children { get; set; }

        public TreeViewEntry(string name, string path) {
            Directory = name;
            Path = path;
            _isNodeExpanded = true;
            _isNodeSelected = false;
            Icon = Properties.Resources.Folder;
            Children = new ObservableCollection<TreeViewEntry>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
