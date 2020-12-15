using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace MessengerLibrary
{
    public class Authanswer
    {
        public string Nicknameuser { get; set; }
        public int Iduser { get; set; }
        public List<(int,string)> Chatnames_Id { get; set; }
        public Authanswer(int iduser, string nicknameuser, List<(int,string)> chatnames)
        {
            this.Iduser = iduser;
            this.Nicknameuser = nicknameuser;
            this.Chatnames_Id = chatnames;
        }
        public Authanswer(string error)
        {
            this.Iduser = 0;
            this.Nicknameuser = error;
            this.Chatnames_Id = new List<(int, string)>();
        }
        public Authanswer(){ }

        public string ToJson() =>
           JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        public Authanswer FromJson(string value) =>
            JsonSerializer.Deserialize<Authanswer>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        public void ToJsonFile(string path) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), this.ToJson());
        public Authanswer FromJsonFile(string path) =>
            this.FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
