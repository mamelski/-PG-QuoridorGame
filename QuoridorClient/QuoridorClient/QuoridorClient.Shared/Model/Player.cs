using System;
using System.Collections.Generic;
using System.Text;
using QuoridorClient.Enums;

namespace QuoridorClient.Model
{
    public class Player
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class PlayerMessage
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        //public MessageType Type { get; set; }
    }
}
