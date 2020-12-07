using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MessengerLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    public struct Getrequestmessage
    {
        public DateTime Daterequest { get; set; }
       public string Nickname { get; set; }
        public Getrequestmessage(DateTime daterequest, string nickname)
        {
            this.Daterequest = daterequest;
            this.Nickname = nickname;
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult Get(int id, [FromBody] Getrequestmessage getrequestmessage)
        {
            //if(Program.Users.Find(user => user.Nickname == getrequestmessage.Nickname).Chats.Exists(chat => chat == id))
            //{
            //    //Возвращает все сообщения надо починить, я устал
            //    List<Message> mes = Program.Chats.Find(chat => chat.IdChat == id).Messages.FindAll(mes => mes.TimeSend < getrequestmessage.Daterequest);
            //    return Ok(mes);
            //}
            return Ok("Ай ай меня обманывать");
        }

        [HttpPost]
        //Тоже надо переписать 
        public IActionResult Post([FromBody]Message message)
        {
            //Message msg = JsonSerializer.Deserialize<Message>(message);
            return Ok(message);
        }
    }
}
