using System.Net.Sockets;

namespace AFTPLib.Core {
    public class ServerClient {

        private Socket _clientSocket;
        
        
        public ServerClient(Socket socket, AftpServer aftpServer) {
            _clientSocket = socket;
        }
    }
}