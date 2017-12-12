using QuoridorClient.Enums;

namespace QuoridorClient.Model
{
    //Sending this event is equal to ending the turn
    public class BoardEvent : GameEvent
    {
        public BoardEvent() : base(GameEventType.BoardChanged) {}

        //public int X { get; set; }
        //public int Y { get; set; }
        public MoveDirection Move { get; set; }
        public int NextPlayerId { get; set; }
        public BoardEventType BoardEventType { get; set; }
    }
}
