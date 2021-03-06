﻿using Microsoft.AspNetCore.Mvc;
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
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Check new message started");
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString()));
            Chat chat = Chat.FromJsonFile(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), $"{id_chat}.json"));
            if (!chat.Members.Exists(mem => mem== getrequestmessage.IdUser))
            {
                logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Chech new message interrupted because this user doesn't belong this chat");
                return Ok("Ne obmanevai mena, teba net v etom chate");
            }
            List<Message> Mess_list = new List<Message>();
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message"));
            foreach (string nam in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message")))
            {
                if(DateTime.Parse(Path.GetFileName(nam).Substring(0, Path.GetFileName(nam).LastIndexOf(".json"))).Date >= getrequestmessage.Last_mess.Date)/////
                {
                    List<Message> mess_list = IMainFunction.FromJsonFile<List<Message>>(nam);
                    Mess_list.InsertRange(0,from mes in mess_list where mes.TimeSend.TimeOfDay > getrequestmessage.Last_mess.TimeOfDay select mes);
                }                         
            }
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + $"Check new message comlete. Send {Mess_list.Count} message");
            return Ok(new CheckMessResponse(Mess_list, chat.Members.Count));
        }
        [HttpPost("oldmes/{id_chat}")]
        [Produces("application/json")]
        public IActionResult Oldmes (int id_chat, [FromBody] Check_message_request getrequestmessage)
        {
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Sample old message started");
            Directory.CreateDirectory(Path.Combine(Program.config["Chats_directory"], id_chat.ToString()));
            Chat chat = Chat.FromJsonFile(Path.Combine(Program.config["Chats_directory"], id_chat.ToString(), $"{id_chat}.json"));
            if (!chat.Members.Exists(mem => mem == getrequestmessage.IdUser))
            {
                logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Chech new message interrupted because this user doesn't belong this chat");
                return Ok("Ne obmanevai mena, teba net v etom chate");
            }
            List<Message> Mess_list = new List<Message>();
            if (getrequestmessage.Last_mess == new DateTime())
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message"));
                List<string> directory_name_files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message")).OrderBy(nam => DateTime.Parse(Path.GetFileName(nam).Substring(0, Path.GetFileName(nam).LastIndexOf(".json")))).ToList<string>();///////
                List<Message> buff_mes = new List<Message>();
                for (int i=1;Mess_list.Count!=10 || i>directory_name_files.Count;i++)
                {
                    try
                    {
                        buff_mes = IMainFunction.FromJsonFile<List<Message>>(directory_name_files[^i]);
                    }
                    catch 
                    {
                        break;
                    }
                    Mess_list.InsertRange(0,buff_mes.TakeLast(10 - Mess_list.Count));
                }
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message"));
                List<string> directory_name_files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message")).OrderBy(nam => DateTime.Parse(Path.GetFileName(nam).Substring(0, Path.GetFileName(nam).LastIndexOf(".json")))).ToList<string>();///////
                List<Message> buff_mes = new List<Message>();
                int index = directory_name_files.IndexOf(directory_name_files.Find(nam => DateTime.Parse(Path.GetFileName(nam).Substring(0, Path.GetFileName(nam).LastIndexOf(".json"))).Date == getrequestmessage.Last_mess.Date));
                for (int i = index; Mess_list.Count != 10 || i<0; i--)
                {
                    try
                    {
                        buff_mes = IMainFunction.FromJsonFile<List<Message>>(directory_name_files[i]);
                    }
                    catch 
                    {
                        break;
                    }

                    Mess_list.InsertRange(0, buff_mes.FindAll(mes => (mes.TimeSend.TimeOfDay < getrequestmessage.Last_mess.TimeOfDay) || (mes.TimeSend.Date < getrequestmessage.Last_mess.Date)).TakeLast(10 - Mess_list.Count));
                }
            }
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + $"Sample old message comlete. Send {Mess_list.Count} message");
            return Ok(new CheckMessResponse(Mess_list, chat.Members.Count));
        }
        [HttpPost("sendmes/{id_chat}")]
        public IActionResult Sendmes(int id_chat, [FromBody]Message message)
        {
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Send message started");
            if (!Program.ChatsID.ContainsKey(id_chat)) 
            {
                logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Send message interrupted because this chat is not found");
                return Ok("Not found this chat");
            }
            if (!Chat.FromJsonFile(Path.Combine(Program.config["Chats_directory"],id_chat.ToString(),$"{id_chat}.json")).Members.Exists(mem => mem == message.Sender))
            {
                logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Send message interrupted because this user not belong this chat");
                return Ok("Po moemu ti ne otcuda");
            }
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message"));    
            if (!System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json")))
                IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json", $"{id_chat}.json"), new List<Message>());
            List<Message> Mess_list = IMainFunction.FromJsonFile<List<Message>>(Path.Combine( Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json"));
            Mess_list.Add(message);
            IMainFunction.ToJsonFile(Path.Combine(Program.config["Chats_directory"], id_chat.ToString(), "history_message", $"{message.TimeSend.ToShortDateString()}.json"), Mess_list);
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Send message complete");
            return Ok("Ok pochta doshla");
        }
        [HttpPost("createchat")]
        [Produces("application/json")]
        public IActionResult CreateChat([FromBody] Create_chat_struct request_create)
        {
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Create chat started");
            Chat chat = new Chat() {
                NameChat = request_create.NameChat,
                Members = new List<int>()
            };
            if (Program.ChatsID.Count == 0) chat.IdChat = 0;
            else chat.IdChat = Program.ChatsID.Keys.Max() + 1;
            foreach (string nick in request_create.Members)
                chat.Members.Add(Program.NickName[nick]);
            Program.ChatsID.Add(chat.IdChat, chat.NameChat);
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Program.config["Chats_directory"], chat.IdChat.ToString()));
            chat.ToJsonFile(Path.Combine(Program.config["Chats_directory"], chat.IdChat.ToString(), $"{chat.IdChat}.json"));
            foreach (int mem in chat.Members) {
                User us = MessengerLibrary.User.FromJsonFile(Path.Combine(Program.config["User_directory"], $"{mem}.json"));
                us.Chats.Add(chat.IdChat);
                us.ToJsonFile(Path.Combine(Program.config["User_directory"], $"{mem}.json"));
            }
            logger.LogInformation($"[{ DateTime.Now.ToString()}]: " + "Create chat comlete");
            return Ok(chat);
        }
    }
}
