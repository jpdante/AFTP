using System.Net.Sockets;

namespace AFTPLib.Net
{
    public class FSocket {

        private Socket _socket;

        public FSocket() {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        
        public bool IsConnected => !((_socket.Poll(1000, SelectMode.SelectRead) && (_socket.Available == 0)) || !_socket.Connected);

    }
}