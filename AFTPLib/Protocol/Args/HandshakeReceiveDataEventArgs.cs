using System;
using System.Globalization;

namespace AFTPLib.Protocol.Args {
    public class HandshakeReceiveDataEventArgs : EventArgs {
        public bool Cancel { get; set; }
        public HandshakeCancelReason CancelReason { get; set; }
        public int Version { get; }
        public string Software { get; }

        public HandshakeReceiveDataEventArgs(bool cancel, HandshakeCancelReason cancelReason, int version, string software) {
            Cancel = cancel;
            CancelReason = cancelReason;
            Version = version;
            Software = software;
        }
    }
}