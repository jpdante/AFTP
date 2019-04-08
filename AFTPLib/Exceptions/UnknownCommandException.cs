using System;
using System.Net;

namespace AFTPLib.Exceptions {
    public class UnknownCommandException : Exception {
        public UnknownCommandException(byte id) : base($"Unknown command '{id}'.") {
        }
    }
}