using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AFTP.Client.Enum;
using AFTP.Client.Model.File;
using Renci.SshNet;

namespace AFTP.Client.Model.Client {
    public class BaseSFtpClient : BaseClient {

        private readonly SftpClient _sftpClient;

        public BaseSFtpClient(string host, int port, string username, string password) {
            _sftpClient = new SftpClient(host, port, username, password);
        }

        public BaseSFtpClient(string host, int port, string username, string privateKey, string privateKeyPassword) {
            _sftpClient = privateKeyPassword != null ?
                new SftpClient(host, port, username, new PrivateKeyFile(privateKey, privateKeyPassword)) :
                new SftpClient(host, port, username, new PrivateKeyFile(privateKey));
        }

        public override event ConnectedHandler OnConnected;
        public override event DisconnectedHandler OnDisconnected;

        public override bool IsConnected => _sftpClient.IsConnected;

        public override Task Connect() {
            _sftpClient.Connect();
            OnConnected?.Invoke(this);
            return Task.CompletedTask;
        }

        public override Task Disconnect() {
            _sftpClient.Disconnect(); 
            OnDisconnected?.Invoke(this);
            return Task.CompletedTask;
        }

        public override void Configure(Dictionary<ServerSettingsType, string> settings) {

        }

        public override Task<bool> UploadFile(string localPath, string remotePath, bool overwrite, bool createFolder, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task CreateDirectory(string path, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task<RemoteEntry[]> ListDirectory(string path, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task DeleteFile(string path, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task DeleteDirectory(string path, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task<bool> FileExists(string path, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task<bool> DirectoryExists(string path, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task Rename(string path, string dest, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task<bool> MoveDirectory(string path, string dest, bool overwrite, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task<bool> MoveFile(string path, string dest, bool overwrite, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override Task<bool> DownloadFileAsync(string localPath, string remotePath, bool overwrite, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public override void Dispose(bool disposing) {
            _sftpClient.Dispose();
        }
    }
}
