using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Events
{
    public class EventRequest
    {
        public EventRequestType RequestType { get; set; }
        public Object RequestObject { get; set; }
    }

    public enum EventRequestType : byte
    {
        LogIn,
        CheckMessages,
        CheckGameEvents,
        SendMessage,
        SendGameEvent,
        GetLoggedUsers,
        CheckAwaitingInvitation,
        Invite,
        AcceptInvitation,
        CheckAcceptedInvitation
    }
}
