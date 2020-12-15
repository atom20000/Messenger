﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace MessengerLibrary
{
    public class Chat
    {
        public string NameChat { get; set; }
        public int IdChat { get; set; }
        public List<int> Members { get; set; }
        public Chat(){ }
        public Chat(int idchat, string namechat, List<int> members)
        {
            this.IdChat = idchat;
            this.Members = members;
            this.NameChat = namechat;
        }
       public string ToJson() =>
            JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        public Chat FromJson(string value) =>
            JsonSerializer.Deserialize<Chat>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        public void ToJsonFile(string path) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), this.ToJson());
        public Chat FromJsonFile(string path) =>
            this.FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
