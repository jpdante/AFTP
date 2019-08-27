using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AFTP.Client.Model.Server;

namespace AFTP.Client.Control {
    /// <summary>
    /// Interaction logic for ServerTreeView.xaml
    /// </summary>
    public partial class ServerTreeView : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _oldText;
        private bool _isInEditMode;
        public bool IsInEditMode {
            get => _isInEditMode;
            set {
                _isInEditMode = value;
                var handler = PropertyChanged;
                handler?.Invoke(this, new PropertyChangedEventArgs("IsInEditMode"));
            }
        }

        public ServerTreeView() {
            InitializeComponent();
        }

        private void editableTextBoxHeader_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var tb = sender as TextBox;
            if (tb != null && !tb.IsVisible) return;
            tb?.Focus();
            tb?.SelectAll();
            _oldText = tb?.Text;
        }

        private void editableTextBoxHeader_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) IsInEditMode = false;
            if (e.Key != Key.Escape) return;
            if (sender is TextBox tb) tb.Text = _oldText;
            IsInEditMode = false;
        }

        private void editableTextBoxHeader_LostFocus(object sender, RoutedEventArgs e) {
            IsInEditMode = false;
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            IsInEditMode = false;
        }

        private void treeView_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.F2) IsInEditMode = true;
        }

        private void textBlockHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount != 2) return;
            if (!FindTreeItem(e.OriginalSource as DependencyObject).IsSelected) return;
            IsInEditMode = true;
            e.Handled = true;
        }

        private static TreeViewItem FindTreeItem(DependencyObject source) {
            while (source != null && !(source is TreeViewItem)) source = VisualTreeHelper.GetParent(source);
            return source as TreeViewItem;
        }

        private void treeView_DragOver(object sender, DragEventArgs e) {
            throw new System.NotImplementedException();
        }

        private void treeView_Drop(object sender, DragEventArgs e) {
            throw new System.NotImplementedException();
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e) {
            throw new System.NotImplementedException();
        }
    }
}
