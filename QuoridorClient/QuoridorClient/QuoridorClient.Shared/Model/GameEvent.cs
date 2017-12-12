using QuoridorClient.Enums;

namespace QuoridorClient.Model
{
    public class GameEvent
    {
        public GameEvent() {}

        public GameEvent(GameEventType eventType)
        {
            EventType = eventType;
        }

        public GameEventType EventType { get; private set; }

        public GameEventType GetGameEventType()
        {
            return EventType;
        }
    }
}
