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
        public List<(int ID_chat ,string Namechat)> Chatnames_Id { get; set; }
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
        public static Authanswer FromJson(string value) =>
            JsonSerializer.Deserialize<Authanswer>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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
        public static Authanswer FromJsonFile(string path) =>
            FromJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
