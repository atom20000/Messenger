using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerLibrary
{
    public class User
    {
        public int IdUser { get; set; }
        public  string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public List<int> Chats { get; set; }
        public User(string login, string password, string nickname)
        {
            this.Login = login;
            this.Password = password;
            this.Nickname = nickname;
            this.Chats = new List<int>();
        }
        public User(){ }
    }
}
