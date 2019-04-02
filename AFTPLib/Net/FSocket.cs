using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AFTPLib.Net
{
    public class FSocket : Socket {
        public FSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType) {
        }

        public FSocket(SocketInformation socketInformation) : base(socketInformation) {
        }

        public FSocket(SocketType socketType, ProtocolType protocolType) : base(socketType, protocolType) {
        }
        
        public bool IsConnected => !((this.Poll(1000, SelectMode.SelectRead) && (this.Available == 0)) || !this.Connected);
    }
}