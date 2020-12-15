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
        public static Create_chat_struct FromJson(string value) =>
            JsonSerializer.Deserialize<Create_chat_struct>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

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
        public static Create_chat_struct FromJsonFile(string path) =>
            FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
