using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MessengerLibrary
{
    public struct CheckMessResponse
    {
        public List<Message> Mess_list { get; set; }
        public int Count_members { get; set; }
        public CheckMessResponse(List<Message> mess_list, int count_members)
        {
            this.Mess_list = mess_list;
            this.Count_members = count_members;
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
        public static CheckMessResponse FromJson(string value) =>
            JsonSerializer.Deserialize<CheckMessResponse>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

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
        public static CheckMessResponse FromJsonFile(string path) =>
            FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
