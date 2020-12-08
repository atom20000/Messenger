using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerLibrary
{
    public class Chat
    {
        public string NameChat { get; set; }
        public int IdChat { get; set; }
        //public List<Message> Messages { get; set; }
        public List<int> Members { get; set; }
        public Chat(){ }
        public Chat(int idchat, string namechat, List<int> members)
        {
            this.IdChat = idchat;
            this.Members = members;
            //this.Messages = new List<Message>();
            this.NameChat = namechat;
        }
    }
}
