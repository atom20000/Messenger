using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using MessengerLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        [HttpGet("chekmes/{id_chat}")]
        [Produces("application/json")]
        public IActionResult Checkmes(int id_chat, [FromBody] Check_message_request getrequestmessage)
        {
            if (!JsonSerializer.Deserialize<Chat>(System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"],id_chat.ToString(),$"{id_chat}.json")), new JsonSerializerOptions() { WriteIndented = true }).Members.Exists(mem => mem== getrequestmessage.IdUser))
                return Ok("Ne obmanevai mena, teba net v etom chate");
            List<Message> Mess_list = new List<Message>();
            foreach (string nam in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message")))
            {
                if(DateTime.Parse(Path.GetFileName(nam).Substring(0, nam.LastIndexOf(".json"))) >= getrequestmessage.Last_mess)
                {
                    List<Message> mess_list = JsonSerializer.Deserialize<List<Message>>(nam);
                    Mess_list.AddRange(from mes in mess_list where mes.TimeSend >= getrequestmessage.Last_mess select mes);
                }                         
            }
            return Ok(Mess_list);
        }
        [HttpPost("sendmes/{id_chat}")]
        public IActionResult Sendmes(int id_chat, [FromBody]Message message)
        {
            if (!Program.ChatsID.ContainsKey(id_chat)) return Ok("Not found this chat");
            if (!JsonSerializer.Deserialize<Chat>(System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),Program.config["Chats_directory"],id_chat.ToString(),$"{id_chat}.json")), new JsonSerializerOptions() { WriteIndented = true }).Members.Exists(mem => mem == message.Sender)) 
                return Ok("Po moemu ti ne otcuda");
            if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(),Program.config["Chats_directory"],id_chat.ToString(),"history_message",$"{message.TimeSend.ToShortDateString()}.json")))
                System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json", $"{id_chat}.json"), JsonSerializer.Serialize(new List<Message>(), new JsonSerializerOptions() { WriteIndented = true }));
            List<Message> Mess_list = JsonSerializer.Deserialize<List<Message>>(System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json")));
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json"), JsonSerializer.Serialize(Mess_list, new JsonSerializerOptions() { WriteIndented = true }));
            return Ok("Ok pochta doshla");
        }
        [HttpPost("createchat")]
        [Produces("application/json")]
        public IActionResult CreateChat([FromBody] Create_chat_struct request_create)
        {
            Chat chat = new Chat();
            chat.NameChat = request_create.NameChat;
            if (Program.ChatsID.Count == 0) chat.IdChat = 0;
            else chat.IdChat = Program.ChatsID.Keys.Max() + 1;
            foreach (string nick in request_create.Members)
                chat.Members.Add(Program.NickName[nick]);
            Program.ChatsID.Add(chat.IdChat, chat.NameChat);
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], chat.IdChat.ToString(), $"{chat.IdChat}.json"), JsonSerializer.Serialize<Chat>(chat, new JsonSerializerOptions() { WriteIndented = true }));
            foreach (int mem in chat.Members) {
               User us = JsonSerializer.Deserialize<User>(System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),Program.config["User_directory"],$"{mem}.json")), new JsonSerializerOptions() { WriteIndented = true });
               us.Chats.Add(chat.IdChat);
               System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), Program.config["User_directory"],$"{mem}.json"), JsonSerializer.Serialize(us, new JsonSerializerOptions() { WriteIndented = true }));
            }
            return Ok(chat);
        }
    }
}
