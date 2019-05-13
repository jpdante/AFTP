using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using AFTPLib.Protocol;
using AFTPLib.Protocol.Args;

namespace AFTPLib.Core {
    public class ServerClient {

        private readonly Socket _clientSocket;
        private readonly AftpServer _aftpServer;
        private Stream _stream;
        private AftpConnection _aftpConnection;

        public ServerClient(Socket socket, AftpServer aftpServer) {
            _clientSocket = socket;
            _aftpServer = aftpServer;
        }

        public void Start() {
            _stream = new NetworkStream(_clientSocket);
            _aftpConnection = new AftpConnection(_stream, new ConnectionSettings() {
                SentVersion = 1,
                UseEncryption = false
            }, true, _aftpServer.ServerConfig.HandshakeTimeout);
            _aftpConnection.OnCheckVersion += OnCheckVersion;
            _aftpConnection.OnErrorOccurs += OnErrorOccurs;
            _aftpConnection.OnRequestSecureStream += OnRequestSecureStream;
            _aftpConnection.OnHandshakeFinish += OnHandshakeFinish;
            _aftpConnection.OnAuthenticationRequest += OnAuthenticationRequest;
            _aftpConnection.Handshake();
        }

        private void OnAuthenticationRequest(object sender, AuthenticationRequestEventArgs args) {
            Console.WriteLine($"[Server] Received authentication request: {args.User} {args.Password} {args.PasswordEncryptionType}");
            args.Success = true;
        }

        private void OnHandshakeFinish(object sender) {
            Console.WriteLine("[Server] Handshake Finished!");
        }

        private Stream OnRequestSecureStream(object sender, Stream stream, bool selfSigned) {
            var sslStream = new SslStream(_stream, false);
            sslStream.AuthenticateAsServer(_aftpServer.ServerConfig.ServerCertificate, false, SslProtocols.Default, true);
            return sslStream;
        }

        private static void OnErrorOccurs(object sender, Exception ex) {
            throw ex;
        }

        private void OnCheckVersion(object sender, CheckVersionEventArgs args) {
            Console.WriteLine($"[Server] Client Version: {args.Version}");
            Console.WriteLine($"[Server] Client Software: {args.Software}");
            if (args.Version <= _aftpServer.ServerConfig.Version) return;
            args.Cancel = true;
            args.CancelReason = HandshakeCancelReason.UnsupportedVersion;
        }
    }
}