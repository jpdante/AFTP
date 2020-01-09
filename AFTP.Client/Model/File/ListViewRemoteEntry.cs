using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFTP.Client.Enum;

namespace AFTP.Client.Model.File {
    public class ListViewRemoteEntry {

        public string FileName { get; private set; }
        public string Size { get; private set; }
        public string Type { get; private set; }
        public string Modified { get; private set; }
        public string Permissions { get; private set; }
        public string Owner { get; private set; }

        public readonly RemoteEntry RemoteEntry;

        public ListViewRemoteEntry(RemoteEntry remoteEntry) {
            RemoteEntry = remoteEntry;
            FileName = remoteEntry.Name;
            Size = remoteEntry.Type != RemoteEntryType.File ? "" : remoteEntry.Size.ToString();
            Type = remoteEntry.Type.ToString();
            Modified = remoteEntry.Modified.ToString("dd/MM/yyyy HH:mm");
            Permissions = "";
            Owner = "";
        }
    }
}
