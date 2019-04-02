using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AFTPLib.Exceptions;
using AFTPLib.Net;

namespace AFTPLib {
    public class AftpClient {

        private readonly FSocket _fSocket;
        private SslStream _stream;

        public AftpClient() {
            _fSocket = new FSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(IPEndPoint ipEndPoint, string user, string password) {
            _fSocket.Connect(ipEndPoint);
            if (!_fSocket.IsConnected) throw new ConnectionFailedException(ipEndPoint.ToString());
            _stream = new SslStream(new NetworkStream(_fSocket), false, 
                new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            _stream.AuthenticateAsClient(ipEndPoint.Address.ToString());  
            
        }

        public async Task ConnectAsync(IPEndPoint ipEndPoint, string user, string password) {
            await _fSocket.ConnectAsync(ipEndPoint);
            if (!_fSocket.IsConnected) throw new ConnectionFailedException(ipEndPoint.ToString());
            _stream = new SslStream(new NetworkStream(_fSocket), false, 
                new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            _stream.AuthenticateAsClient(ipEndPoint.Address.ToString());         
        }

        public void Disconnect() {
            
        }

        public async Task DisconnectAsync() {
            
        }
        
        

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors) {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

    }
}
