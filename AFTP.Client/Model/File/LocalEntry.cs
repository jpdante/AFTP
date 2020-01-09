using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFTP.Client.Enum;

namespace AFTP.Client.Model.File {
    public class LocalEntry {

        public string Name { get; set; }
        public string FullName { get; set; }
        public long Size { get; set; }
        public RemoteEntryType Type { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public string LinkTarget { get; set; }
    }
}
