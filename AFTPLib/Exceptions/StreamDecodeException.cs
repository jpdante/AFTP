using System;
using System.Collections.Generic;
using System.Text;

namespace AFTPLib.Exceptions {
    public class StreamDecodeException : Exception {
        public StreamDecodeException() : base("Failed to decode stream.") {
        }
    }
}
