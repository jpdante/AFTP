using System.Diagnostics;

namespace AFTP.Client.Model.Util {
    public class LogTraceListener : TraceListener {

        public delegate void WriteHandler(object sender, string data);
        public static event WriteHandler OnWrite;

        public delegate void WriteLineHandler(object sender, string data);
        public static event WriteLineHandler OnWriteLine;

        public override void Write(string message) { OnWrite?.Invoke(this, message); }

        public override void WriteLine(string message) { OnWriteLine?.Invoke(this, message); }
    }
}
