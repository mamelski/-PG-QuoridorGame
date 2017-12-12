using QuoridorClient.Enums;

namespace QuoridorClient.Model
{
    public class GameInvitation : GameEvent
    {
        public GameInvitation() : base(GameEventType.GameInvitation)
        {
        }

        public int InvitingPlayerId { get; set; }
    }

    public class GameInvitationResponse : GameEvent
    {
        public GameInvitationResponse() : base(GameEventType.GameInvitationResponse)
        {
        }

        public int ResponderId { get; set; }
        public bool InvitationAccepted { get; set; }
    }
}
