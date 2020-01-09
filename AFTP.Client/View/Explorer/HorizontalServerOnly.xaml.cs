using System.Windows.Controls;
using System.Windows.Input;
using AFTP.Client.Model.File;
using AFTP.Client.Model.View;

namespace AFTP.Client.View.Explorer {
    /// <summary>
    /// Interaction logic for HorizontalServerOnly.xaml
    /// </summary>
    public partial class HorizontalServerOnly : FileExplorer {
        public HorizontalServerOnly() {
            InitializeComponent();
        }

        public override void UpdateRemoteEntries(RemoteEntry[] remoteEntries) {
            RemoteListView.Items.Clear();
            foreach (var entry in remoteEntries) {
                RemoteListView.Items.Add(new ListViewRemoteEntry(entry));   
            }
        }

        public override void UpdateLocalEntries(LocalEntry[] localEntries) {

        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e) {
            var listViewRemoteEntry = ((ListViewItem) sender).Content as ListViewRemoteEntry;

        }
    }
}
