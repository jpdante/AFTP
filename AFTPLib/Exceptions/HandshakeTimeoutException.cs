using System;
using System.Collections.Generic;
using System.Text;

namespace AFTPLib.Exceptions {
    class HandshakeTimeoutException : Exception {
        public HandshakeTimeoutException() : base("Handshake failed, timeout.") {
        }
    }
}
