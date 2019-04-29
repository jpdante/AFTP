using System;

namespace AFTPLib.Protocol.Args {
    public class AuthenticationReceiveDataEventArgs : EventArgs {
        public bool Authenticated { get; set; }
        public string Username { get; }
        public string Password { get; }
        public PasswordEncryption EncryptionAlgorithm { get; }
        
        public AuthenticationReceiveDataEventArgs(bool authenticated, string username, string password, PasswordEncryption encryptionAlgorithm) {
            Authenticated = authenticated;
            Username = username;
            Password = password;
            EncryptionAlgorithm = encryptionAlgorithm;
        }
    }

    public enum PasswordEncryption {
        PlainText, SHA256, SHA512, 
    }
}