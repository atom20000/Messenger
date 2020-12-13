using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerLibrary
{
    public class Sing_out_request
    {
        public string NickName { get; set; }
        public List<int> Chats_Id { get; set; }
        public DateTime Sing_Out_Time { get; set; }
        public Sing_out_request(string nickname, List<int> chats_id, DateTime sing_out_time)
        {
            this.NickName = nickname;
            this.Chats_Id = chats_id;
            this.Sing_Out_Time = sing_out_time;
        }
    }
}
