using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AFTPLib.Exceptions;

namespace AFTPLib {
    public class AftpClient {

        private Socket _socket;

        public AftpClient() {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        #region Connect

        public void Connect(EndPoint endPoint) {
            _socket.Connect(endPoint);
            if (!IsConnected) throw new ConnectionFailedException(endPoint.ToString());
        }

        public async Task ConnectAsync(EndPoint endPoint) {
            await _socket.ConnectAsync(endPoint);
            if (!IsConnected) throw new ConnectionFailedException(endPoint.ToString());
        }

        #endregion
        
        #region Commands
        
        
        
        #endregion

        #region Helpers

        public bool IsConnected => !((_socket.Poll(1000, SelectMode.SelectRead) && (_socket.Available == 0)) || !_socket.Connected);

        #endregion

    }
}
