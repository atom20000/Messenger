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
            if (Program.LoginID.ContainsKey(user.Login))
            {
                logger.LogInformation("Registration interrupted because this login is registered");
                return Ok(new Authanswer("This login is registered"));
            }

            if (Program.NickName.Keys.ToList().Exists(us => us == user.Nickname)) 
            {
                logger.LogInformation("Registration interrupted because this nickname is busy");
                return Ok(new Authanswer("This nickname is busy")); 
            }
            if(Program.LoginID.Count==0) user.IdUser = 0;  
            else user.IdUser = Program.LoginID.Values.Max() + 1;

            ///////////////////////////////////////////////После сдачи удалить
            user.Chats.Add(0);
            Chat chat = Chat.FromJsonFile(Path.Combine(Program.config["Chats_directory"], "0", $"{0}.json"));
            chat.Members.Add(user.IdUser);
            chat.ToJsonFile(Path.Combine(Program.config["Chats_directory"], "0", $"{0}.json"));
            ////////////////////////////////////////////////После сдачи удалить

            Program.NickName.Add(user.Nickname, user.IdUser);
            Program.LoginID.Add(user.Login, user.IdUser);
            user.ToJsonFile(Path.Combine(Program.config["User_directory"], $"{user.IdUser}.json"));
            logger.LogInformation("Registration complete");
            return Ok(new Authanswer("Davai cherez auth"));
        }
        [HttpPost("auth")]
        [Produces("application/json")]
        public IActionResult Auth([FromBody] List<string> request)
        {
            logger.LogInformation("Authorization starts");
            if (!Program.LoginID.ContainsKey(request[0]))
            {
                logger.LogInformation("Authorization interrupted because login not found");
                return Ok(new Authanswer("Login not found"));
            } 
            int iduser = Program.LoginID[request[0]];
            User user = MessengerLibrary.User.FromJsonFile(Path.Combine(Program.config["User_directory"], $"{iduser}.json"));
            if (user.Password == request[1]) {
                List<(int, string)> chatnames_id = new List<(int, string)>();
                foreach (int chat in user.Chats)
                {
                    chatnames_id.Add((chat, Program.ChatsID[chat]));
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], chat.ToString(),"history_message"));
                    if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], chat.ToString(),
                        "history_message", $"{DateTime.Now.ToUniversalTime().ToShortDateString()}.json")))
                            IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], chat.ToString(), "history_message",
                                $"{DateTime.Now.ToUniversalTime().ToShortDateString()}.json", $"{chat}.json"),
                                new List<Message>() { new Message(DateTime.Now.ToUniversalTime(), $"{user.Nickname} online", -9999, "Ttechnical information") });  
                    else
                    {
                        List<Message> Mess_list = IMainFunction.FromJsonFile<List<Message>>(Path.Combine(Program.config["Chats_directory"], chat.ToString(),
                            "history_message", $"{DateTime.Now.ToUniversalTime().ToShortDateString()}.json"));
                        Mess_list.Add(new Message(DateTime.Now.ToUniversalTime(), $"{user.Nickname} online", -9999, "Ttechnical information"));
                        IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], chat.ToString(), "history_message", $"{DateTime.Now.ToUniversalTime().ToShortDateString()}.json"), Mess_list);
                    }
                }
                logger.LogInformation("Authorization complete");
                return Ok(new Authanswer(user.IdUser, user.Nickname, chatnames_id));
             }
            logger.LogInformation("Authorization interrupted because password invalid");
            return Ok(new Authanswer("Password invalid"));
        }
        [HttpPost("singout")]
        [Produces("application/json")]
        public IActionResult Sing_out([FromBody] Sing_out_request out_request)
        {
            logger.LogInformation("Sign out starts");
            //Напиши проверку что пользователь есть в этих беседах
            foreach (int chat in out_request.Chats_Id)
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], chat.ToString(), "history_message"));
                if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], chat.ToString(), "history_message", $"{out_request.Sing_Out_Time.ToShortDateString()}.json")))
                    IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], chat.ToString(), "history_message", $"{out_request.Sing_Out_Time.ToShortDateString()}.json", $"{chat}.json"),
                        new List<Message>() { new Message(out_request.Sing_Out_Time, $"{out_request.NickName} offline", -9999, "Ttechnical information") });
                else
                {
                    List<Message> Mess_list = IMainFunction.FromJsonFile<List<Message>>(Path.Combine(Program.config["Chats_directory"], chat.ToString(), "history_message", $"{out_request.Sing_Out_Time.ToShortDateString()}.json")); 
                    Mess_list.Add(new Message(out_request.Sing_Out_Time, $"{out_request.NickName} offline", -9999, "Ttechnical information"));
                    IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], chat.ToString(), "history_message", $"{out_request.Sing_Out_Time.ToShortDateString()}.json"), Mess_list);
                }
            }
            logger.LogInformation("Sign out complete");
            return Ok("vse ok");
        }
    }
}
