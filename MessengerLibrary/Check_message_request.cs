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
        public static Check_message_request FromJson(string value) =>
            JsonSerializer.Deserialize<Check_message_request>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

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
        public static Check_message_request FromJsonFile(string path) =>
            FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
