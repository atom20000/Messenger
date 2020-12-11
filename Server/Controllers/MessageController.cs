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
    public struct Getrequestmessage
    {
        public DateTime Last_mess { get; set; }
        public int IdUser { get; set; }
        public Getrequestmessage(DateTime last_mess, int iduser)
        {
            this.Last_mess = last_mess;
            this.IdUser = iduser;
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        [HttpGet("chekmes/{id_chat}")]
        [Produces("application/json")]
        public IActionResult Checkmes(int id_chat, [FromBody] Getrequestmessage getrequestmessage)
        {
            if (!JsonSerializer.Deserialize<Chat>(System.IO.File.ReadAllText($"{Program.config["Chats_directory"]}\\{id_chat}.json"), new JsonSerializerOptions() { WriteIndented = true }).Members.Exists(mem => mem== getrequestmessage.IdUser)) return Ok("Ne obmanevai mena, teba net v etom chate");

            //    //Возвращает все сообщения надо починить, я устал
            //    List<Message> mes = Program.Chats.Find(chat => chat.IdChat == id).Messages.FindAll(mes => mes.TimeSend < getrequestmessage.Daterequest);
            //    return Ok(mes);
            //}
            return Ok("Ай ай меня обманывать");
        }
        [HttpPost("sendmes/{id_chat}")]
        public IActionResult Sendmes(int id_chat, [FromBody]Message message)
        {
            if (!Program.ChatsID.ContainsKey(id_chat)) return Ok("Not found this chat");
            if (!JsonSerializer.Deserialize<Chat>(System.IO.File.ReadAllText($"{Program.config["Chats_directory"]}\\{id_chat}.json"), new JsonSerializerOptions() { WriteIndented = true }).Members.Exists(mem => mem == message.Sender)) return Ok("Po moemu ti ne otcuda");
            if (!Directory.Exists($"{Program.config["Chats_directory"]}\\history_message\\{message.TimeSend.ToShortDateString()}"))
                Directory.CreateDirectory($"{Program.config["Chats_directory"]}\\history_message\\{message.TimeSend.ToShortDateString()}");
            List<Message> Mess_list = JsonSerializer.Deserialize<List<Message>>(System.IO.File.ReadAllText($"{Program.config["Chats_directory"]}\\history_message\\{message.TimeSend.ToShortDateString()}\\{id_chat}.json"));
            System.IO.File.WriteAllText($"{Program.config["Chats_directory"]}\\history_message\\{message.TimeSend.ToShortDateString()}\\{id_chat}.json", JsonSerializer.Serialize<List<Message>>(Mess_list, new JsonSerializerOptions() { WriteIndented = true }));
            // подумай AllText или AllLines 
            return Ok("Ok pochta doshla");
        }
        [HttpPost("createchat")]
        [Produces("application/json")]
        public IActionResult CreateChat([FromBody] Chat chat)
        {
            if (Program.ChatsID.Count == 0) chat.IdChat = 0;
            else chat.IdChat = Program.ChatsID.Keys.Max() + 1;
            Program.ChatsID.Add(chat.IdChat, chat.NameChat);
            System.IO.File.WriteAllText($"{Program.config["Chats_directory"]}\\{chat.IdChat}.json", JsonSerializer.Serialize<Chat>(chat, new JsonSerializerOptions() { WriteIndented = true }));
            foreach (int mem in chat.Members) {
               User us = JsonSerializer.Deserialize<User>(System.IO.File.ReadAllText($"{Program.config["User_directory"]}\\{mem}.json"), new JsonSerializerOptions() { WriteIndented = true });
               us.Chats.Add(chat.IdChat);
               System.IO.File.WriteAllText($"{Program.config["User_directory"]}\\{mem}.json", JsonSerializer.Serialize(us, new JsonSerializerOptions() { WriteIndented = true }));
            }
            return Ok(chat);
        }
    }
}
