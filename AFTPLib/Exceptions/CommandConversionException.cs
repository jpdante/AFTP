using System;
using System.Collections.Generic;
using System.Text;

namespace AFTPLib.Exceptions {
    public class CommandConversionException : Exception {
        public CommandConversionException() : base("Failed to convert command, the command is different than expected.") {
        }
    }
}
