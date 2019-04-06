using ProtoBuf;

namespace AFTPLib.Protocol.Commands {
    [ProtoContract]
    public class UserAuthentication : ProtoStream {

        [ProtoMember(2)] public string User;
        [ProtoMember(3)] public string Password;

        public UserAuthentication() : base(3) {

        }

        public UserAuthentication(string user, string password) : base(1) {
            User = user;
            Password = password;
        }
    }
}