using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerLibrary
{
    public class Authanswer
    {
        public string Nicknameuser { get; set; }
        public int Iduser { get; set; }
        public List<(int,string)> Chatnames_Id { get; set; }
        public Authanswer(int iduser, string nicknameuser, List<(int,string)> chatnames)
        {
            this.Iduser = iduser;
            this.Nicknameuser = nicknameuser;
            this.Chatnames_Id = chatnames;
        }
        public Authanswer(string error)
        {
            this.Iduser = 0;
            this.Nicknameuser = error;
            this.Chatnames_Id = new List<(int, string)>();
        }
        public Authanswer(){ }
    }
}
