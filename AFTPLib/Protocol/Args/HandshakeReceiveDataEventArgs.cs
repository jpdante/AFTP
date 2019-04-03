using System;

namespace AFTPLib.Protocol.Args {
    public class HandshakeReceiveDataEventArgs : EventArgs {
        public int Version { get; }
        public string Software { get; }
        public bool Cancel { get; set; }
        public HandshakeCancelReason CancelReason { get; set; }

        public HandshakeReceiveDataEventArgs(bool cancel, HandshakeCancelReason cancelReason, int version = 0, string software = null) {
            Version = version;
            Software = software;
            Cancel = cancel;
            CancelReason = cancelReason;
        }
        
    }

    public enum HandshakeCancelReason {
        UnsupportedVersion, UnsupportedSoftware, Unknown
    }
}