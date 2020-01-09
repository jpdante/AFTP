using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using AFTP.Client.Enum;
using AFTP.Client.Model.Utils;
using FluentFTP;

namespace AFTP.Client.Model.Client {
    public class BaseFtpClient : BaseClient {

        private FtpClient _ftpClient;

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

        public override void Configure(Dictionary<ServerSettingsType, string> settings) {
            if (settings.TryGetValue(ServerSettingsType.TransferMode, out var transferMode)) {
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
            if (settings.TryGetValue(ServerSettingsType.CharsetEncoding, out var charsetEncoding)) {
                switch (charsetEncoding) {
                    case "default":
                        //_ftpClient.Encoding = Encoding.ASCII;
                        break;
                    default:
                        _ftpClient.Encoding = Encoding.GetEncoding(charsetEncoding);
                        break;
                }
            }
            if (settings.TryGetValue(ServerSettingsType.Encryption, out var encryption)) {
                switch (encryption) {
                    case "try-explicit-tls":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.Explicit;
                        _ftpClient.SslProtocols = SslProtocols.Tls;
                        break;
                    case "require-explicit-tls":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.Explicit;
                        _ftpClient.SslProtocols = SslProtocols.Tls;
                        break;
                    case "require-implicit-tls":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.Implicit;
                        _ftpClient.SslProtocols = SslProtocols.Tls;
                        break;
                    case "plain":
                        _ftpClient.EncryptionMode = FtpEncryptionMode.None;
                        _ftpClient.SslProtocols = SslProtocols.None;
                        break;
                    default:
                        _ftpClient.EncryptionMode = FtpEncryptionMode.None;
                        break;
                    /*default:
                        _ftpClient.Encoding = Encoding.GetEncoding(charsetEncoding);
                        break;*/
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
        }

        public override async Task Disconnect() {
            await _ftpClient.DisconnectAsync();
        }

        public override Task UploadFile() {
            return null;
        }

        public override Task CreateFile() {
            return null;
        }

        public override Task CreateDirectory() {
            return null;
        }

        public override Task ListDirectory() {
            return null;
        }

        public override Task DeleteFile() {
            return null;
        }

        public override Task DeleteDirectory() {
            return null;
        }

        public override Task FileExists() {
            return null;
        }

        public override Task DirectoryExists() {
            return null;
        }

        public override Task Rename() {
            return null;
        }
    }
}
