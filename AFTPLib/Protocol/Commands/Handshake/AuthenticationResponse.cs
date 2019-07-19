using ProtoBuf;

namespace AFTPLib.Protocol.Commands.Handshake {
    [ProtoContract]
    public class AuthenticationResponse : ProtoStream {

        [ProtoMember(2)] public bool Success;

        public AuthenticationResponse() : base(7) {

        }

        public AuthenticationResponse(bool success) : base(7) {
            Success = success;
        }
    }
}