namespace QuoridorService.DataContracts
{
    public class PlayerMessage
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        //public MessageType Type { get; set; }
    }
}
