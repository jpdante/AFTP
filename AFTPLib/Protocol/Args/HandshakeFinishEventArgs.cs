using System;

namespace AFTPLib.Protocol.Args {
    public class HandshakeFinishEventArgs : EventArgs {

        public bool Success { get; }
        public Exception Exception { get; }

        public HandshakeFinishEventArgs(bool success, Exception ex) {
            Success = success;
            Exception = ex;
        }
        
    }
}