using System;
using System.Net;

namespace AFTPLib.Exceptions {
    public class HandshakeErrorException : Exception {
        public HandshakeErrorException() : base("Handshake failed, invalid socket response.") {
        }
    }
}