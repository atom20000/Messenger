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
        public List<Message> Messages { get; set; }
        public List<int> Members { get; set; }
        public Chat(){ }
        //internal Chat(string namechat, List<User> members)
        //{
        //    if (Program.Chats.Count == 0)
        //    {
        //        this.IdChat = 0;
        //        this.Members = members;
        //        this.Messages = new List<Message>();
        //        this.NameChat = namechat;
        //    }
        //    List<Chat> BuffChat = Program.Chats;
        //    BuffChat.Sort((chat1, chat2) => chat1.IdChat.CompareTo(chat2.IdChat));
        //    this.IdChat = BuffChat.Last().IdChat + 1;
        //    this.Members = members;
        //    this.Messages = new List<Message>();
        //    this.NameChat = namechat;
        //}
    }
}
