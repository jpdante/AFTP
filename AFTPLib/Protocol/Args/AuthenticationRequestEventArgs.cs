using System;

namespace AFTPLib.Protocol.Args {
    public class AuthenticationRequestEventArgs : EventArgs {
        public readonly string User;
        public readonly string Password;
        public bool Success;

        public AuthenticationRequestEventArgs(string user, string password) {
            User = user;
            Password = password;
            Success = false;
        }
        
    }
}