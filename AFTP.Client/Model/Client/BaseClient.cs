using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFTP.Client.Enum;

namespace AFTP.Client.Model.Client {
    public abstract class BaseClient {

        protected BaseClient() {

        }

        public abstract void Configure(Dictionary<ServerSettingsType, string> settings);
        public abstract Task Connect();
        public abstract Task Disconnect();
        public abstract Task UploadFile();
        public abstract Task CreateFile();
        public abstract Task CreateDirectory();
        public abstract Task ListDirectory();
        public abstract Task DeleteFile();
        public abstract Task DeleteDirectory();
        public abstract Task FileExists();
        public abstract Task DirectoryExists();
        public abstract Task Rename();
    }
}
