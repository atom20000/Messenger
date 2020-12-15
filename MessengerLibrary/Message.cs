using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
        public string ToJson() =>
            JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        public Message FromJson(string value) =>
            JsonSerializer.Deserialize<Message>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        public void ToJsonFile(string path) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), this.ToJson());
        public Message FromJsonFile(string path) =>
            this.FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
