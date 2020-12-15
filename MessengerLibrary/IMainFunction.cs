using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

namespace MessengerLibrary
{
     public interface IMainFunction
    {
        /// <summary>
        /// Преобразует объект в Json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(object value) => 
            JsonSerializer.Serialize(value, new JsonSerializerOptions() { WriteIndented = true });

        /// <summary>
        /// Преобразует из Json в объект <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T FromJson<T>(string value) => 
            JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        /// <summary>
        /// Преобразует объект в Json и сохраняет в файл
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void ToJsonFile(string path, object value) =>
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), path), ToJson(value));

        /// <summary>
        /// Считывает из файла json и преобразует в объект <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T FromJsonFile<T>(string path) =>
            FromJson<T>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path)));
    }
}
