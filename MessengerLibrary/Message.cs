using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerLibrary
{
    public class Message
    {
        public DateTime TimeSend { get; set; }
        public string Text { get; set; }
        public int Sender { get; set; }
        public string Sender_Nickname { get; set; }
        public Message(DateTime timesend, string text, int sender, string sender_nickname)
        {
            this.TimeSend = timesend;
            this.Text = text;
            this.Sender = sender;
            this.Sender_Nickname = sender_nickname;
        }
        public Message() { }
    }
}
