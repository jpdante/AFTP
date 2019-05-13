using System;

namespace AFTPLib.Protocol.Args {
    public class AuthenticationResponseEventArgs : EventArgs {
        public bool Success;

        public AuthenticationResponseEventArgs(bool success) {
            Success = success;
        }
        
    }
}