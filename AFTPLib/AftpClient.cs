using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AFTPLib.Configuration;
using AFTPLib.Exceptions;
using AFTPLib.Net;
using AFTPLib.Protocol;
using AFTPLib.Protocol.Args;

namespace AFTPLib {
    public class AftpClient {

        private readonly FSocket _fSocket;
        private readonly ClientConfig _clientConfig;
        private Stream _stream;
        private AftpConnection _aftpConnection;
        private string _targetHost;
        private string _username;
        private string _password;

        public AftpClient(ClientConfig clientConfig) {
            _clientConfig = clientConfig;
            _fSocket = new FSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string address, int port, string username, string password) {
            _fSocket.Connect(address, port);
            _targetHost = address;
            _username = username;
            _password = password;
            Console.WriteLine("[Client] Socket connected!");
            if (!_fSocket.IsConnected) throw new ConnectionFailedException(address);
            _stream = new NetworkStream(_fSocket);
            _aftpConnection = new AftpConnection(_stream, new ConnectionSettings() {
                SentVersion = 1
            }, false, 1);
            _aftpConnection.OnCheckVersion += OnCheckVersion;
            _aftpConnection.OnErrorOccurs += OnErrorOccurs;
            _aftpConnection.OnRequestSecureStream += OnRequestSecureStream;
            _aftpConnection.OnHandshakeFinish += OnHandshakeFinish;
            _aftpConnection.OnAuthenticationResult += OnAuthenticationResult;
            _aftpConnection.Handshake();
        }

        private void OnAuthenticationResult(object sender, AuthenticationResponseEventArgs args) {
            Console.WriteLine($"[Client] Authentication finished! Success: {args.Success}");
        }

        private void OnHandshakeFinish(object sender) {
            Console.WriteLine("[Client] Handshake Finished!");
            Console.WriteLine("[Client] Starting authentication...");
            _aftpConnection.Authenticate(_username, _password);
        }

        private Stream OnRequestSecureStream(object sender, Stream stream, bool selfSigned) {
            var sslStream = new SslStream(_stream, false, new RemoteCertificateValidationCallback((o, certificate, chain, errors) => {
                if (_clientConfig.DisableCertificateCheck) return true;
                if (_clientConfig.AllowSelfSignedBypass && selfSigned) return true;
                return errors == SslPolicyErrors.None;
            }), null);
            sslStream.AuthenticateAsClient(_targetHost);
            return sslStream;
        }

        private static void OnErrorOccurs(object sender, Exception ex) {
            throw ex;
        }

        private void OnCheckVersion(object sender, CheckVersionEventArgs args) {
            Console.WriteLine($"[Client] Server Version: {args.Version}");
            Console.WriteLine($"[Client] Server Software: {args.Software}");
            if (args.Version <= _clientConfig.Version) return;
            args.Cancel = true;
            args.CancelReason = HandshakeCancelReason.UnsupportedVersion;
        }

        /*private readonly FSocket _fSocket;
        private readonly ClientConfig _clientConfig;
        private SslStream _stream;
        private Handshaking _handshaking;

        public AftpClient(ClientConfig clientConfig) {
            _clientConfig = clientConfig;
            _fSocket = new FSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string address, int port, string user, string password) {
            _fSocket.Connect(address, port);
            Console.WriteLine("[Client] Socket connected!");
            if (!_fSocket.IsConnected) throw new ConnectionFailedException(address);
            _stream = new SslStream(new NetworkStream(_fSocket), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            _stream.AuthenticateAsClient(address);
            Console.WriteLine("[Client] Ssl stream set.");
            _handshaking = new Handshaking(_stream, false,0,_clientConfig.Version, _clientConfig.Software);
            _handshaking.OnHandshakeReceiveData += OnHandshakeReceiveData;
            _handshaking.OnHandshakeFinish += (sender, args) => {
                if (!args.Success) {
                    _stream.Close();
                    _fSocket.Disconnect(true);
                    throw args.Exception;
                }
                Console.WriteLine("Handshake Success!");
                Authenticate(user, password);
            };
            _handshaking.StartHandshake();
        }

        public async Task ConnectAsync(string address, int port, string user, string password) {
            await _fSocket.ConnectAsync(address, port);
            if (!_fSocket.IsConnected) throw new ConnectionFailedException(address);
            _stream = new SslStream(new NetworkStream(_fSocket), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            _stream.AuthenticateAsClient(address);
            _handshaking = new Handshaking(_stream, false, 0, _clientConfig.Version, _clientConfig.Software);
            _handshaking.OnHandshakeReceiveData += OnHandshakeReceiveData;
            _handshaking.OnHandshakeFinish += (sender, args) => {
                if (!args.Success) {
                    _stream.Close();
                    _fSocket.Disconnect(true);
                    throw args.Exception;
                }
                Authenticate(user, password);
            };
            _handshaking.StartHandshake();
        }
        
        public void Disconnect() {
            _fSocket.Disconnect(true);
        }   

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            if (_clientConfig.DisableCertificateCheck) return true;
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        private void OnHandshakeReceiveData(object sender, HandshakeReceiveDataEventArgs args) {
            Console.WriteLine($"[Client] Server Version: {args.Version}");
            Console.WriteLine($"[Client] Server Software: {args.Software}");
            if (args.Version <= _clientConfig.Version) return;
            args.Cancel = true;
            args.CancelReason = HandshakeCancelReason.UnsupportedVersion;
        }

        private void Authenticate(string user, string password) {
            Console.WriteLine($"[Client] Authenticating...");
            var authentication = new Authentication(_stream, false);
            authentication.StartAuthentication("usuario", "senha");
        }*/

    }
}
