using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Other {
    [ProtoContract]
    public class EndConnection : ProtoStream {
        
        [ProtoMember(2)] public byte Reason;
        [ProtoMember(3)] public string Message;
        
        public EndConnection() : base(4) {
        }
        
        public EndConnection(EndConnectionReason reason, string message = null) : base(4) {
            Reason = (byte)reason;
            Message = message;
        }
    }

    public enum EndConnectionReason : byte {
        Unknown = 0,
        Disconnect = 1,
        HandshakeError = 2,
        AuthenticationError = 3,
        AuthenticationTimeout = 4,
        InvalidCommand = 5,
        UnsupportedVersion = 6,
        UnsupportedSoftware = 7,
        
    }
}