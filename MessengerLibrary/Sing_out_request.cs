using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MessengerLibrary
{
    public class Sing_out_request 
    {
        public string NickName { get; set; }
        public List<int> Chats_Id { get; set; }
        public DateTime Sing_Out_Time { get; set; }
        public Sing_out_request(string nickname, List<int> chats_id, DateTime sing_out_time)
        {
            this.NickName = nickname;
            this.Chats_Id = chats_id;
            this.Sing_Out_Time = sing_out_time;
        }
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
        public Sing_out_request FromJson(string value) =>
            JsonSerializer.Deserialize<Sing_out_request>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

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
        public Sing_out_request FromJsonFile(string path) =>
            this.FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
