using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once InconsistentNaming

namespace AFTP.Client.Model.EventArg {
    public class FileTransferEventArgs : EventArgs {

        public double Progress { get; set; }
        public double TransferSpeed { get; set; }
        public TimeSpan ETA  { get; set; }
        public string LocalPath  { get; set; }
        public string RemotePath  { get; set; }
        public Guid Guid  { get; set; }

    }
}
