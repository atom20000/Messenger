using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerLibrary
{
   public struct Create_chat_struct
    {
        public string NameChat { get; set; }
        public List<string> Members { get; set; }]
        public Create_chat_struct( string namechat, List<string> members)
        {
            this.NameChat = namechat;
            this.Members = members;
        }
    }
}
