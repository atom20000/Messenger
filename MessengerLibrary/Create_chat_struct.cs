using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MessengerLibrary
{
   public struct Create_chat_struct 
    {
        public string NameChat { get; set; }
        public List<string> Members { get; set; }
        public Create_chat_struct( string namechat, List<string> members)
        {
            this.NameChat = namechat;
            this.Members = members;
        }
        public string ToJson() =>
            JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        public Create_chat_struct FromJson(string value) =>
            JsonSerializer.Deserialize<Create_chat_struct>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        public void ToJsonFile(string path) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), this.ToJson());
        public Create_chat_struct FromJsonFile(string path) =>
            this.FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
