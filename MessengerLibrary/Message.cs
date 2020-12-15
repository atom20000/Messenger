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

        /// <summary>
        /// Преобразует объект в Json
        /// </summary>
        /// <returns></returns>
        public string ToJson() =>
            JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });

        /// <summary>
        /// реобразует из Json в объект
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Message FromJson(string value) =>
            JsonSerializer.Deserialize<Message>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        /// <summary>
        /// Преобразует объект в Json и сохраняет в файл
        /// </summary>
        /// <param name="path"></param>
        public void ToJsonFile(string path) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), this.ToJson());

        /// <summary>
        /// Считывает из файла json и преобразует в объект
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Message FromJsonFile(string path) =>
            FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
