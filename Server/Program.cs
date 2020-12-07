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
        public static void Main(string[] args)
        {
            if (!Directory.Exists(@"Users")) Directory.CreateDirectory(@"Users");
            if (!Directory.Exists(@"Chats")) Directory.CreateDirectory(@"Chats");
            if (!File.Exists(@"Users\nickname.json")) File.WriteAllText(@"Users\nickname.json", JsonConvert.SerializeObject(NickName, Formatting.Indented));
            if (!File.Exists(@"Users\loginid.json")) File.WriteAllText(@"Users\loginid.json", JsonConvert.SerializeObject(LoginID, Formatting.Indented));
            if (!File.Exists(@"Chats\chatsid.json")) File.WriteAllText(@"Chats\chatsid.json", JsonConvert.SerializeObject(ChatsID, Formatting.Indented));

            NickName = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(@"Users\nickname.json"));
            LoginID = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(@"Users\loginid.json"));
            ChatsID = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(@"Chats\chatsid.json"));

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
