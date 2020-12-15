using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerLibrary
{
    public struct CheckMessResponse
    {
        public List<Message> Mess_list { get; set; }
        public int Count_members { get; set; }
        public CheckMessResponse(List<Message> mess_list, int count_members)
        {
            this.Mess_list = mess_list;
            this.Count_members = count_members;
        }
    }
}
