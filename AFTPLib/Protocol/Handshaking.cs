using System.IO;

namespace AFTPLib.Protocol
{
    public class Handshaking {
        private Stream _stream;

        public delegate void HandshakeFinishHandler(object sender, bool success);

        public HandshakeFinishHandler OnHandshakeFinish;

        public Handshaking(Stream stream) {
            _stream = stream;
        }

        public void Handshake() {
            
        }
        
        public void HandshakeAsync() {

        }
    }
}