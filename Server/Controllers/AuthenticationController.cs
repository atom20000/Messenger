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
            if (new List<string>(Program.LoginID.Keys).Exists(us => us == user.Login)) return Ok(new Authanswer("This login is registered"));
            if (Program.NickName.Exists(us => us==user.Nickname)) return Ok(new Authanswer("This nickname is busy"));
            if(Program.LoginID.Count==0) user.IdUser = 0;  
            else user.IdUser = Program.LoginID.Values.Max() + 1;
            Program.NickName.Add(user.Nickname);
            Program.LoginID.Add(user.Login, user.IdUser);
            // добавить что чувак в сети 
            System.IO.File.WriteAllText($"{Program.config["User_directory"]}\\{user.IdUser.ToString()}.json", JsonSerializer.Serialize<User>(user));
            logger.LogInformation("Registration complete");
            return Ok(new Authanswer(user.IdUser,user.Nickname, new List<string>(),user.Chats));
        }
        [HttpPost("auth")]
        [Produces("application/json")]
        public IActionResult Auth([FromBody] List<string> request)
        {
            logger.LogInformation("Authorization starts");
            int iduser = Program.LoginID[request[0]];
            if (!System.IO.File.Exists($"{Program.config["User_directory"]}\\{iduser}.json")) return Ok(new Authanswer("Login not found"));
            User user = JsonSerializer.Deserialize<User>(System.IO.File.ReadAllText($"{Program.config["User_directory"]}\\{iduser}.json"), new JsonSerializerOptions() { WriteIndented = true });
            if (user.Password == request[1]) {
                List<string> chatnames = new List<string>();
                foreach (int chat in user.Chats) chatnames.Add(Program.ChatsID[chat]);
                logger.LogInformation("Authorization complete");
                return Ok(new Authanswer(user.IdUser, user.Nickname, chatnames, user.Chats));
             }
            return Ok(new Authanswer("Password invalid"));
        }
    }
}
