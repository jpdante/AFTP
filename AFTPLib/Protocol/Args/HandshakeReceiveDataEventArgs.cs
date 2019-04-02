using System;

namespace AFTPLib.Protocol.Args {
    public class HandshakeReceiveDataEventArgs : EventArgs {
        public int Version { get; }
        public byte SystemType { get; }
        public string SystemName { get; }
        public bool Cancel { get; set; }
        public HandshakeCancelReason CancelReason { get; set; }

        public HandshakeReceiveDataEventArgs(int version = 0, byte systemType = 0, string systemName = null) {
            Version = version;
            SystemType = systemType;
            SystemName = systemName;
        }
        
    }

    public enum HandshakeCancelReason {
        UnsupportedVersion, UnsupportedSystem, Unknown
    }
}