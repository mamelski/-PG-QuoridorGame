using System;
using System.Collections.Generic;
using System.Text;

namespace QuoridorClient.Enums
{
    public enum MessageType : byte
    {
        TextMessage,
        GameEvent
    }

    public enum GameEventType : byte
    {
        Event,
        GameInvitation,
        GameInvitationResponse,
        BoardChanged,
        EndGame
    }

    public enum BoardEventType : byte
    {
        WallPlaced,
        PlayerMoved
    }
}
