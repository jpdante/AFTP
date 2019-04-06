using System;

namespace AFTPLib.Protocol.Args {
    public class CheckVersionEventArgs : EventArgs {
        public readonly int Version;
        public readonly string Software;
        public bool Cancel;
        public HandshakeCancelReason CancelReason;

        public CheckVersionEventArgs(int version = 0, string software = null) {
            Version = version;
            Software = software;
            Cancel = false;
            CancelReason = HandshakeCancelReason.Unknown;
        }
        
    }

    public enum HandshakeCancelReason {
        UnsupportedVersion, UnsupportedSoftware, Unknown
    }
}