using System;
using System.Collections.Generic;
using System.Text;
using Quoridor.DataContracts;
using Quoridor.Enums;

namespace Quoridor.Events
{
    public class EventBase
    {
        public string EventMessage { get; set; }
    }

    public class LogInEvent : EventBase
    {
        public bool IsLogged { get; set; }
        public Player Me { get; set; }
    }

    public class MessagesInboundEvent : EventBase
    {
        public List<PlayerMessage> Messages { get; set; } 
    }

    public class LoggedUsersRefreshed : EventBase
    {
        public List<Player> LoggedUsers { get; set; } 
    }

    public class InvitationHandshake : EventBase
    {
        public Invitation Invitation { get; set; }
    }

    public class MoveEventArgs : EventBase
    {
        public Move Move { get; set; }
    }
}
