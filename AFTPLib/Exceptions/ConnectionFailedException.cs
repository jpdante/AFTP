using System;
using System.Collections.Generic;
using System.Text;

namespace AFTPLib.Exceptions {
    public class ConnectionFailedException : Exception {
        public ConnectionFailedException(string host)
            : base($"Failed to connect to server \"{host}\".") {

        }
    }
}
