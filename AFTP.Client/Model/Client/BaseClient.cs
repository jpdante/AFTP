using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AFTP.Client.Enum;
using AFTP.Client.Model.EventArg;
using AFTP.Client.Model.File;

namespace AFTP.Client.Model.Client {
    public abstract class BaseClient : IDisposable {

        //public delegate void FileTransferUpdateHandler(object sender, FileTransferEventArgs e);
        //public event FileTransferUpdateHandler OnUpdateStatus;
        public delegate void ConnectedHandler(object sender);
        public delegate void DisconnectedHandler(object sender);

        public abstract event ConnectedHandler OnConnected;
        public abstract event DisconnectedHandler OnDisconnected;

        public abstract bool IsConnected { get; }

        public abstract void Configure(Dictionary<ServerSettingsType, string> settings);
        public abstract Task Connect();
        public abstract Task Disconnect();
        public abstract Task<bool> UploadFile(string localPath, string remotePath, bool overwrite, bool createFolder, CancellationToken cancellationToken);
        public abstract Task CreateDirectory(string path, CancellationToken cancellationToken);
        public abstract Task<RemoteEntry[]> ListDirectory(string path, CancellationToken cancellationToken);
        public abstract Task DeleteFile(string path, CancellationToken cancellationToken);
        public abstract Task DeleteDirectory(string path, CancellationToken cancellationToken);
        public abstract Task<bool> FileExists(string path, CancellationToken cancellationToken);
        public abstract Task<bool> DirectoryExists(string path, CancellationToken cancellationToken);
        public abstract Task Rename(string path, string dest, CancellationToken cancellationToken);
        public abstract Task<bool> MoveDirectory(string path, string dest, bool overwrite, CancellationToken cancellationToken);
        public abstract Task<bool> MoveFile(string path, string dest, bool overwrite, CancellationToken cancellationToken);
        public abstract Task<bool> DownloadFileAsync(string localPath, string remotePath, bool overwrite, CancellationToken cancellationToken);
        public abstract void Dispose(bool disposing);

        public void Dispose() { Dispose(true); }
    }
}
