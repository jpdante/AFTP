using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AFTPLib.Configuration;
using AFTPLib.Managers;
using AFTPLib.Net;

namespace AFTPLib {
    public class AftpServer {

        private readonly FSocket _fSocket;
        private readonly ServerConfig _serverConfig;
        private readonly UserManager _userManager;
        private readonly FirewallManager _firewallManager;
        private bool _isListening;

        public AftpServer(ServerConfig serverConfig) {
            _serverConfig = serverConfig;
            _fSocket = new FSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _fSocket.Bind(_serverConfig.Address);
            _userManager = new UserManager();
            _firewallManager = new FirewallManager();
        }

        public void Start() {
            _isListening = true;
            _fSocket.Listen(_serverConfig.BackLog);
            _firewallManager.Start();
            _fSocket.BeginAccept(EndAcceptSocket, null);
        }

        private void EndAcceptSocket(IAsyncResult ar) {
            try {
                Socket socket = _fSocket.EndAccept(ar);
                int timeout = 0;
                if (_firewallManager.AllowConnection(((IPEndPoint) socket.RemoteEndPoint).Address, out timeout)) {
                    
                } else {
                    
                }
            } catch {
                // ignored
            }
            if(_isListening) _fSocket.BeginAccept(EndAcceptSocket, null);
        }

        public void Stop() {
            _isListening = false;
            _fSocket.Disconnect(true);
            _firewallManager.Stop();
        }
    }
}
