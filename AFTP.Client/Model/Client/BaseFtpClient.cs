using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AFTP.Client.Enum;
using AFTP.Client.Model.File;
using AFTP.Client.Model.Util;
using FluentFTP;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AFTP.Client.Model.Client {
    public class BaseFtpClient : BaseClient {

        private readonly FtpClient _ftpClient;

        static BaseFtpClient() {
            FtpTrace.AddListener(new LogTraceListener());
            FtpTrace.LogUserName = true;
            FtpTrace.LogPassword = false;
            FtpTrace.LogIP = true; 
        }

        public BaseFtpClient(string host, int port, string username, string password) {
            _ftpClient = new FtpClient(host, port, username, password);
            _ftpClient.ValidateCertificate += FtpClientOnValidateCertificate;
        }

        private void FtpClientOnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e) { e.Accept = e.PolicyErrors == System.Net.Security.SslPolicyErrors.None; }

        public override event ConnectedHandler OnConnected;
        public override event DisconnectedHandler OnDisconnected;

        public override bool IsConnected => _ftpClient.IsConnected;

        public override void Configure(Dictionary<ServerSettingsType, string> settings) {
            if (settings.TryGetValue(ServerSettingsType.TransferMode, out string transferMode)) {
                switch (transferMode) {
                    case "default":
                        _ftpClient.DataConnectionType = FtpDataConnectionType.AutoActive;
                        break;
                    case "active":
                        _ftpClient.DataConnectionType = FtpDataConnectionType.AutoActive;
                        break;
                    case "passive":
                        _ftpClient.DataConnectionType = FtpDataConnectionType.AutoPassive;
                        break;
                    default:
                        _ftpClient.DataConnectionType = FtpDataConnectionType.AutoPassive;
                        break;
                }
            }
            if (settings.TryGetValue(ServerSettingsType.CharsetEncoding, out string charsetEncoding)) {
                switch (charsetEncoding) {
                    case "auto-detect":
                        break;
                    default:
                        _ftpClient.Encoding = Encoding.GetEncoding(charsetEncoding);
                        break;
                }
            }
            if (settings.TryGetValue(ServerSettingsType.Encryption, out string encryption)) {
                switch (encryption) {
                    case "try-explicit-tls":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.Explicit;
                        _ftpClient.SslProtocols = SslProtocols.Tls | SslProtocols.None;// | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.None;
                        break;
                    case "require-explicit-tls":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.Explicit;
                        _ftpClient.SslProtocols = SslProtocols.Tls;// | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.None;
                        break;
                    case "require-implicit-tls":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.Implicit;
                        _ftpClient.SslProtocols = SslProtocols.Tls;// | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Ssl2 | SslProtocols.Ssl3;
                        break;
                    case "plain":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.None;
                        _ftpClient.SslProtocols = SslProtocols.None;
                        break;
                    default:
                        _ftpClient.EncryptionMode = FtpEncryptionMode.Explicit;
                        _ftpClient.SslProtocols = SslProtocols.Tls;//| SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.None;
                        break;
                }
            }
            _ftpClient.ConnectTimeout = 5000;
            _ftpClient.ReadTimeout = 5000;
            _ftpClient.DataConnectionConnectTimeout = 5000;
            _ftpClient.DataConnectionReadTimeout  = 5000;
            _ftpClient.SocketPollInterval  = 5000;
        }

        public override async Task Connect() {
            await _ftpClient.ConnectAsync();
            OnConnected?.Invoke(this);
        }

        public override async Task Disconnect() {
            await _ftpClient.DisconnectAsync();
            OnDisconnected?.Invoke(this);
        }

        public override async Task<bool> UploadFile(string localPath, string remotePath, bool overwrite, bool createFolder, CancellationToken cancellationToken) {
            var progress = new Progress<FtpProgress>(x => {
                
            });
            return await _ftpClient.UploadFileAsync(localPath, remotePath, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip, createFolder, FtpVerify.None, progress, cancellationToken);
        }

        public override async Task CreateDirectory(string path, CancellationToken cancellationToken) {
            await _ftpClient.CreateDirectoryAsync(path, cancellationToken);
        }

        public override async Task<RemoteEntry[]> ListDirectory(string path, CancellationToken cancellationToken) {
            var listing = await _ftpClient.GetListingAsync(path, FtpListOption.Auto, cancellationToken);
            var list = new List<RemoteEntry>();
            foreach (var item in listing) {
                var entry = new RemoteEntry {
                    Name = item.Name,
                    FullName = item.FullName,
                    Size = item.Size,
                    Modified = item.Modified,
                    Created = item.Created,
                    LinkTarget = item.LinkTarget
                };
                switch (item.Type) {
                    case FtpFileSystemObjectType.Directory:
                        entry.Type = RemoteEntryType.Directory;
                        break;
                    case FtpFileSystemObjectType.File:
                        entry.Type = RemoteEntryType.File;
                        break;
                    case FtpFileSystemObjectType.Link:
                        entry.Type = RemoteEntryType.Link;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                list.Add(entry);
            }
            return list.ToArray();
        }

        public override async Task DeleteFile(string path, CancellationToken cancellationToken) {
            await _ftpClient.DeleteFileAsync(path, cancellationToken);
        }

        public override async Task DeleteDirectory(string path, CancellationToken cancellationToken) {
            await _ftpClient.DeleteDirectoryAsync(path, cancellationToken);
        }

        public override async Task<bool> FileExists(string path, CancellationToken cancellationToken) {
            return await _ftpClient.FileExistsAsync(path, cancellationToken);
        }

        public override async Task<bool> DirectoryExists(string path, CancellationToken cancellationToken) {
            return await _ftpClient.DirectoryExistsAsync(path, cancellationToken);
        }

        public override async Task Rename(string path, string dest, CancellationToken cancellationToken) {
            await _ftpClient.RenameAsync(path, dest, cancellationToken);
        }

        public override async Task<bool> MoveDirectory(string path, string dest, bool overwrite, CancellationToken cancellationToken) {
            return await _ftpClient.MoveDirectoryAsync(path, dest, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip, cancellationToken);
        }

        public override async Task<bool> MoveFile(string path, string dest, bool overwrite, CancellationToken cancellationToken) {
            return await _ftpClient.MoveDirectoryAsync(path, dest, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip, cancellationToken);
        }

        public override async Task<bool> DownloadFileAsync(string localPath, string remotePath, bool overwrite, CancellationToken cancellationToken) {
            var progress = new Progress<FtpProgress>(x => {

            });
            return await _ftpClient.DownloadFileAsync(localPath, remotePath, overwrite ? FtpLocalExists.Overwrite : FtpLocalExists.Skip, FtpVerify.None, progress, cancellationToken);
        }

        public override void Dispose(bool disposing) {
            if (disposing) {
                _ftpClient?.Dispose();
            }
        }
    }
}
