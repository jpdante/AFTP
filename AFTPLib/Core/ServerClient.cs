using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using AFTPLib.Protocol;
using AFTPLib.Protocol.Args;

namespace AFTPLib.Core {
    public class ServerClient {

        private readonly Socket _clientSocket;
        private readonly AftpServer _aftpServer;
        private SslStream _sslStream;

        public ServerClient(Socket socket, AftpServer aftpServer) {
            _clientSocket = socket;
            _aftpServer = aftpServer;
        }

        public void Handshake() {
            _sslStream = new SslStream(new NetworkStream(_clientSocket), false);
            _sslStream.AuthenticateAsServer(_aftpServer.ServerConfig.ServerCertificate, false, SslProtocols.Default, true);
            Handshaking handshaking = new Handshaking(_sslStream, true, _aftpServer.ServerConfig.HandshakeTimeout, _aftpServer.ServerConfig.Version, _aftpServer.ServerConfig.Software);
            handshaking.OnHandshakeReceiveData += OnHandshakeReceiveData;
            handshaking.OnHandshakeFinish += OnHandshakeFinish;
            handshaking.StartHandshake();
        }

        private void OnHandshakeReceiveData(object sender, HandshakeReceiveDataEventArgs args) {
            Console.WriteLine($"[Server] Version: {args.Version}");
            Console.WriteLine($"[Server] Software: {args.Software}");
            if (args.Version <= _aftpServer.ServerConfig.Version) return;
            args.Cancel = true;
            args.CancelReason = HandshakeCancelReason.UnsupportedVersion;
        }

        private void OnHandshakeFinish(object sender, HandshakeFinishEventArgs args) {
            if (args.Success) {
                _aftpServer.ActiveClients.Add(this);
                Authentication authentication = new Authentication(_sslStream, true);
                authentication.StartAuthentication();
            } else throw args.Exception;
        }
    }
}