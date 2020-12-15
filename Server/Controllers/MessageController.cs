using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using MessengerLibrary;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private static readonly ILogger logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<MessageController>();
        [HttpPost("checknewmes/{id_chat}")]
        [Produces("application/json")]
        public IActionResult Checkmes(int id_chat, [FromBody] Check_message_request getrequestmessage)
        {
            logger.LogInformation("Check new message started");
            Chat chat = Chat.FromJsonFile(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), $"{id_chat}.json"));
            if (!chat.Members.Exists(mem => mem== getrequestmessage.IdUser))
            {
                logger.LogInformation("Chech new message interrupted because this user doesn't belong this chat");
                return Ok("Ne obmanevai mena, teba net v etom chate");
            }
            List<Message> Mess_list = new List<Message>();
            foreach (string nam in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message")))
            {
                if(DateTime.Parse(Path.GetFileName(nam).Substring(0, nam.LastIndexOf(".json"))) >= getrequestmessage.Last_mess)
                {
                    List<Message> mess_list = JsonSerializer.Deserialize<List<Message>>(nam);
                    Mess_list.AddRange(from mes in mess_list where mes.TimeSend >= getrequestmessage.Last_mess select mes);
                }                         
            }
            logger.LogInformation("Check new message comlete. Send {Mess_list.Count} message");
            return Ok(new CheckMessResponse(Mess_list, chat.Members.Count));
        }
        [HttpPost("oldmes/{id_chat}")]
        [Produces("application/json")]
        public IActionResult Oldmes (int id_chat, [FromBody] Check_message_request getrequestmessage)
        {
            logger.LogInformation("Sample old message started");
            Chat chat = Chat.FromJsonFile(Path.Combine(Program.config["Chats_directory"], id_chat.ToString(), $"{id_chat}.json"));
            if (!chat.Members.Exists(mem => mem == getrequestmessage.IdUser))
            {
                logger.LogInformation("Chech new message interrupted because this user doesn't belong this chat");
                return Ok("Ne obmanevai mena, teba net v etom chate");
            }
            List<Message> Mess_list = new List<Message>();
            if (getrequestmessage.Last_mess == new DateTime())
            {
                List<string> directory_name_files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message")).OrderBy(nam => DateTime.Parse(Path.GetFileName(nam).Substring(0, nam.LastIndexOf(".json")))).ToList<string>();
                List<Message> buff_mes = new List<Message>();
                for (int i=1;Mess_list.Count!=10;i++)
                {
                    try
                    {
                        buff_mes = IMainFunction.FromJsonFile<List<Message>>(directory_name_files[^i]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        break;
                    }
                    Mess_list.AddRange(buff_mes.TakeLast(10 - Mess_list.Count));
                }
            }
            else
            {
                List<string> directory_name_files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message")).OrderBy(nam => DateTime.Parse(Path.GetFileName(nam).Substring(0, nam.LastIndexOf(".json")))).ToList<string>();
                List<Message> buff_mes = new List<Message>();
                int index = directory_name_files.IndexOf(directory_name_files.Find(nam => DateTime.Parse(Path.GetFileName(nam).Substring(0, nam.LastIndexOf(".json"))).Date == getrequestmessage.Last_mess.Date));
                for (int i = index; Mess_list.Count != 10; i--)
                {
                    try
                    {
                        buff_mes = IMainFunction.FromJsonFile<List<Message>>(directory_name_files[i]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        break;
                    }

                    Mess_list.AddRange(buff_mes.FindAll(mes => mes.TimeSend.TimeOfDay < getrequestmessage.Last_mess.TimeOfDay).TakeLast(10 - Mess_list.Count));
                }
            }
            logger.LogInformation($"Sample old message comlete. Send {Mess_list.Count} message");
            return Ok(new CheckMessResponse(Mess_list, chat.Members.Count));
        }
        [HttpPost("sendmes/{id_chat}")]
        public IActionResult Sendmes(int id_chat, [FromBody]Message message)
        {
            logger.LogInformation("Send message started");
            if (!Program.ChatsID.ContainsKey(id_chat)) 
            {
                logger.LogInformation("Send message interrupted because this chat is not found");
                return Ok("Not found this chat");
            }
            if (!Chat.FromJsonFile(Path.Combine(Program.config["Chats_directory"],id_chat.ToString(),$"{id_chat}.json")).Members.Exists(mem => mem == message.Sender))
            {
                logger.LogInformation("Send message interrupted because this user not belong this chat");
                return Ok("Po moemu ti ne otcuda");
            }
                
            if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json")))
                IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json", $"{id_chat}.json"), new List<Message>());
            List<Message> Mess_list = IMainFunction.FromJsonFile<List<Message>>(Path.Combine( Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json"));
            IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json"), Mess_list);
            logger.LogInformation("Send message complete");
            return Ok("Ok pochta doshla");
        }
        [HttpPost("createchat")]
        [Produces("application/json")]
        public IActionResult CreateChat([FromBody] Create_chat_struct request_create)
        {
            logger.LogInformation("Create chat started");
            Chat chat = new Chat() { NameChat = request_create.NameChat };
            if (Program.ChatsID.Count == 0) chat.IdChat = 0;
            else chat.IdChat = Program.ChatsID.Keys.Max() + 1;
            foreach (string nick in request_create.Members)
                chat.Members.Add(Program.NickName[nick]);
            Program.ChatsID.Add(chat.IdChat, chat.NameChat);
            chat.ToJsonFile(Path.Combine(Program.config["Chats_directory"], chat.IdChat.ToString(), $"{chat.IdChat}.json"));
            foreach (int mem in chat.Members) {
                User us = MessengerLibrary.User.FromJsonFile(Path.Combine(Program.config["User_directory"], $"{mem}.json"));
                us.Chats.Add(chat.IdChat);
                us.ToJsonFile(Path.Combine(Program.config["User_directory"], $"{mem}.json"));
            }
            logger.LogInformation("Create chat comlete");
            return Ok(chat);
        }
    }
}
