using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
                    Icon = GetExtensionBitmap(FileName);
                    break;
            }
            Size = remoteEntry.Type != RemoteEntryType.File ? "" : remoteEntry.Size.ToString();
            Type = remoteEntry.Type.ToString();
            Modified = remoteEntry.Modified.ToString("dd/MM/yyyy HH:mm");
            Permissions = "";
            Owner = "";
            IsBack = false;
        }

        private static Bitmap GetExtensionBitmap(string fileName) {
            switch (Path.GetExtension(fileName)?.ToLower()) {
                case ".png":
                    return Properties.Resources.FilePNG;
                case ".ai":
                    return Properties.Resources.FileAI;
                case ".cdr":
                    return Properties.Resources.FileCDR;
                case ".css":
                    return Properties.Resources.FileCSS;
                case ".doc":
                    return Properties.Resources.FileDoc;
                case ".flv":
                    return Properties.Resources.FileFLV;
                case ".gif":
                    return Properties.Resources.FileGIF;
                case ".html":
                    return Properties.Resources.FileHTML;
                case ".iso":
                    return Properties.Resources.FileISO;
                case ".jpg":
                    return Properties.Resources.FileJPG;
                case ".mkv":
                    return Properties.Resources.FileMKV;
                case ".pdf":
                    return Properties.Resources.FilePDF;
                case ".psd":
                    return Properties.Resources.FilePSD;
                case ".swf":
                    return Properties.Resources.FileSWF;
                case ".txt":
                    return Properties.Resources.FileTXT;
                case ".xls":
                    return Properties.Resources.FileXLS;
                case ".zip":
                    return Properties.Resources.FileZIP;
                case ".php":
                    return Properties.Resources.FilePHP;
                default:
                    return Properties.Resources.File;
            }
        }
    }
}
