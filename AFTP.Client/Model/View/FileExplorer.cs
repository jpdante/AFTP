using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using AFTP.Client.Model.File;

namespace AFTP.Client.Model.View {
    public abstract partial class FileExplorer : Page {

        public abstract string RemotePath { get; set; }
        public abstract string LocalPath { get; set; }

        public abstract void UpdateRemoteEntries(RemoteEntry[] remoteEntries);
        public abstract void UpdateLocalEntries(LocalEntry[] localEntries);

    }
}
