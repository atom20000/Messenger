using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace MessengerLibrary
{
    public class User
    {
        public int IdUser { get; set; }
        public  string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public List<int> Chats { get; set; }
        public User(string login, string password, string nickname)
        {
            this.Login = login;
            this.Password = password;
            this.Nickname = nickname;
            this.Chats = new List<int>();
        }
        public User(){ }
        public string ToJson() =>
            JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        public User FromJson(string value) =>
            JsonSerializer.Deserialize<User>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        public void ToJsonFile(string path) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), this.ToJson());
        public User FromJsonFile(string path) =>
            this.FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
