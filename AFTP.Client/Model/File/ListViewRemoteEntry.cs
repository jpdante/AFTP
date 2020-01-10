using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFTP.Client.Enum;

namespace AFTP.Client.Model.File {

    // TODO: Add "Icon made by DinosoftLabs from www.flaticon.com" to about program.

    public class ListViewRemoteEntry {

        public string FileName { get; private set; }
        public Bitmap Icon { get; private set; }
        public string Size { get; private set; }
        public string Type { get; private set; }
        public string Modified { get; private set; }
        public string Permissions { get; private set; }
        public string Owner { get; private set; }
        public bool IsBack { get; private set; }

        public readonly RemoteEntry RemoteEntry;

        public ListViewRemoteEntry() {
            FileName = "..";
            Icon = Properties.Resources.Folder;
            IsBack = true;
        }

        public ListViewRemoteEntry(RemoteEntry remoteEntry) {
            RemoteEntry = remoteEntry;
            FileName = remoteEntry.Name;
            switch (remoteEntry.Type) {
                case RemoteEntryType.Directory:
                    Icon = Properties.Resources.Folder;
                    break;
                case RemoteEntryType.Link:
                    Icon = Properties.Resources.Link;
                    break;
                default:
                    Icon = Properties.Resources.File;
                    break;
            }
            Size = remoteEntry.Type != RemoteEntryType.File ? "" : remoteEntry.Size.ToString();
            Type = remoteEntry.Type.ToString();
            Modified = remoteEntry.Modified.ToString("dd/MM/yyyy HH:mm");
            Permissions = "";
            Owner = "";
            IsBack = false;
        }
    }
}
