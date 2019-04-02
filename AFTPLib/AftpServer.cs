using System.Net;
using System.Net.Sockets;

namespace AFTPLib {
    public class AftpServer {

        private Socket _socket;
        private readonly int _backlog;

        public AftpServer(EndPoint address, int backlog = 10) {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(address);
            _backlog = backlog;
        }

/// <summary>
/// Start AFTP Server.
/// </summary>
        public void Start() {
            _socket.Listen(_backlog);
        }

        public void Stop() {
            _socket.Disconnect(true);
        }
    }
}
