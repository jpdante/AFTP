using System;
using AFTPLib.Protocol.Args;

namespace AFTPLib.Exceptions {
    public class HandshakeCanceledException : Exception {

        public HandshakeCancelReason CancelReason { get; }

        public HandshakeCanceledException(HandshakeCancelReason cancelReason) {
            CancelReason = cancelReason;
        }
        
    }
}