using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server
{
    public class Program
    {
        internal static List<string> NickName = new List<string>();
        internal static Dictionary<string, int> LoginID = new Dictionary<string, int>();
        internal static Dictionary<int, string> ChatsID = new Dictionary<int, string>();
        internal static Dictionary<string, string> config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"config.json"));
        public static void Main(string[] args)
        {

            if (!Directory.Exists(config["User_directory"])) Directory.CreateDirectory(config["User_directory"]);
            if (!Directory.Exists(config["Chats_directory"])) Directory.CreateDirectory(config["Chats_directory"]);
            if (!Directory.Exists($"{config["Chats_directory"]}\\history_message")) Directory.CreateDirectory($"{config["Chats_directory"]}\\history_message");
            if (!File.Exists($"{config["User_directory"]}\\nickname.json")) File.WriteAllText($"{config["User_directory"]}\\nickname.json", JsonConvert.SerializeObject(NickName, Formatting.Indented));
            if (!File.Exists($"{config["User_directory"]}\\loginid.json")) File.WriteAllText($"{config["User_directory"]}\\loginid.json", JsonConvert.SerializeObject(LoginID, Formatting.Indented));
            if (!File.Exists($"{config["Chats_directory"]}\\chatsid.json")) File.WriteAllText($"{config["Chats_directory"]}\\chatsid.json", JsonConvert.SerializeObject(ChatsID, Formatting.Indented));

            NickName = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText($"{config["User_directory"]}\\nickname.json"));
            LoginID = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText($"{config["User_directory"]}\\loginid.json"));
            ChatsID = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText($"{config["Chats_directory"]}\\chatsid.json"));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls(config["url_host"]);
                });
    }
}
