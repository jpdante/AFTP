using System;

namespace AFTPLib.Protocol.Args {
    public class AuthenticationRequestEventArgs : EventArgs {
        public readonly string User;
        public readonly string Password;
        public readonly PasswordEncryptionType PasswordEncryptionType;
        public bool Success;

        public AuthenticationRequestEventArgs(string user, string password, PasswordEncryptionType passwordEncryptionType) {
            User = user;
            Password = password;
            Success = false;
            PasswordEncryptionType = passwordEncryptionType;
        }
        
    }
}