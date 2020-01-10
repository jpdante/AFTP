using System;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using AFTP.Client.Model.File;
using AFTP.Client.Model.View;

namespace AFTP.Client.Model.View {
    public abstract class FileExplorer : Page {

        public delegate void GoToRemotePathHandler(object sender, string path);
        public delegate void GoToLocalPathHandler(object sender, string path);

        public abstract event GoToRemotePathHandler OnGoToRemotePath;
        public abstract event GoToLocalPathHandler OnGoToLocalPath;

        public abstract void UpdateRemotePath(string path);
        public abstract void UpdateLocalPath(string path);

        public abstract void UpdateRemoteEntries(RemoteEntry[] remoteEntries);
        public abstract void UpdateLocalEntries(LocalEntry[] localEntries);

    }
}
