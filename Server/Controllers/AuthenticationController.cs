using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using MessengerLibrary;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
       private static readonly ILogger logger = LoggerFactory.Create(builder =>
         {
             builder.AddConsole();
         }).CreateLogger<AuthenticationController>();
        [HttpPost("reg")]
        [Produces("application/json")]
        public IActionResult Reg([FromBody] User user)
        {
            logger.LogInformation("Registration starts");
            if (Program.LoginID.ContainsKey(user.Login)) return Ok(new Authanswer("This login is registered"));
            if (Program.NickName.Exists(us => us==user.Nickname)) return Ok(new Authanswer("This nickname is busy"));
            if(Program.LoginID.Count==0) user.IdUser = 0;  
            else user.IdUser = Program.LoginID.Values.Max() + 1;
            Program.NickName.Add(user.Nickname);
            Program.LoginID.Add(user.Login, user.IdUser);
            // добавить что чувак в сети 
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["User_directory"],$"{user.IdUser}.json"), JsonSerializer.Serialize(user, new JsonSerializerOptions() { WriteIndented = true }));
            logger.LogInformation("Registration complete");
            return Ok(new Authanswer(user.IdUser,user.Nickname, new List<(int, string)>(),user.Chats));
        }
        [HttpPost("auth")]
        [Produces("application/json")]
        public IActionResult Auth([FromBody] List<string> request)
        {
            logger.LogInformation("Authorization starts");
            if(!Program.LoginID.ContainsKey(request[0])) return Ok(new Authanswer("Login not found"));
            int iduser = Program.LoginID[request[0]];
            User user = JsonSerializer.Deserialize<User>(System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),Program.config["User_directory"],$"{iduser}.json")), new JsonSerializerOptions() { WriteIndented = true });
            if (user.Password == request[1]) {
                List<(int, string)> chatnames_id = new List<(int, string)>();
                foreach (int chat in user.Chats) chatnames_id.Add((chat,Program.ChatsID[chat]));
                logger.LogInformation("Authorization complete");
                return Ok(new Authanswer(user.IdUser, user.Nickname, chatnames_id, user.Chats));
             }
            return Ok(new Authanswer("Password invalid"));
        }
    }
}
