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
        public string Sender { get; set; }
        public Message(DateTime timesend, string text, string sender)
        {
            this.TimeSend = timesend;
            this.Text = text;
            this.Sender = sender;
        }
        public Message() { }
    }
}
