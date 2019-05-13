using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class RequestAuthentication : ProtoStream {

        [ProtoMember(2)] public string User;
        [ProtoMember(3)] public string Password;

        public RequestAuthentication() : base(6) {

        }

        public RequestAuthentication(string user, string password) : base(6) {
            User = user;
            Password = password;
        }
    }
}