using System;
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
        private SslStream _stream;
        private Handshaking _handshaking;

        public AftpClient(ClientConfig clientConfig) {
            _clientConfig = clientConfig;
            _fSocket = new FSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string address, int port, string user, string password) {
            _fSocket.Connect(address, port);
            if (!_fSocket.IsConnected) throw new ConnectionFailedException(address);
            _stream = new SslStream(new NetworkStream(_fSocket), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            _stream.AuthenticateAsClient(address);
            _handshaking = new Handshaking(_stream, false,0,_clientConfig.Version, _clientConfig.Software);
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
            if (args.Version <= _clientConfig.Version) return;
            args.Cancel = true;
            args.CancelReason = HandshakeCancelReason.UnsupportedVersion;
        }

        private void Authenticate(string user, string password) {

        }

    }
}
