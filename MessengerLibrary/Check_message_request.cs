using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerLibrary
{
    public struct Check_message_request
    {
        public DateTime Last_mess { get; set; }
        public int IdUser { get; set; }
        public Check_message_request(DateTime last_mess, int iduser)
        {
            this.Last_mess = last_mess;
            this.IdUser = iduser;
        }
    }
}
