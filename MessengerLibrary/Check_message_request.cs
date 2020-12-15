using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace MessengerLibrary
{
    public struct Check_message_request 
    {
        public DateTime Last_mess { get; set; }
        public int IdUser { get; set; }
        public Check_message_request(DateTime last_mess, int iduser)
        {
            this.Last_mess = last_mess;
            this.IdUser = iduser;
        }
        public string ToJson() =>
            JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        public Check_message_request FromJson(string value) =>
            JsonSerializer.Deserialize<Check_message_request>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        public void ToJsonFile(string path) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), this.ToJson());
        public Check_message_request FromJsonFile(string path) =>
            this.FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
