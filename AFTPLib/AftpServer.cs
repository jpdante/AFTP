using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Tasks;
using AFTPLib.Configuration;
using AFTPLib.Core;
using AFTPLib.Managers;
using AFTPLib.Net;
using AFTPLib.Protocol;

namespace AFTPLib {
    public class AftpServer {

        public ServerConfig ServerConfig { get; set; }
        internal List<ServerClient> ActiveClients { get; }

        private readonly FSocket _fSocket;
        private readonly UserManager _userManager;
        private readonly FirewallManager _firewallManager;
        private bool _isListening;

        public AftpServer(ServerConfig serverConfig) {
            ServerConfig = serverConfig;
            _fSocket = new FSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _fSocket.Bind(ServerConfig.IpEndPoint);
            _userManager = new UserManager();
            _firewallManager = new FirewallManager(ServerConfig);
            ActiveClients = new List<ServerClient>();
        }

        public void Start() {
            _isListening = true;
            _fSocket.Listen(ServerConfig.BackLog);
            _firewallManager.Start();
            _fSocket.BeginAccept(EndAcceptSocket, null);
        }

        private void EndAcceptSocket(IAsyncResult ar) {
            try {
                Socket socket = _fSocket.EndAccept(ar);
                if (_firewallManager.AllowConnection(((IPEndPoint) socket.RemoteEndPoint).Address)) {
                    ServerClient serverClient = new ServerClient(socket, this);
                    serverClient.Handshake();
                } else {
                    socket.Close();
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
