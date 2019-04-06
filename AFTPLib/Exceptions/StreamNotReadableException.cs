using System;
using System.Collections.Generic;
using System.Text;

namespace AFTPLib.Exceptions {
    public class StreamNotReadableException : Exception {
        public StreamNotReadableException() : base("Stream not readable.") {
        }
    }
}
