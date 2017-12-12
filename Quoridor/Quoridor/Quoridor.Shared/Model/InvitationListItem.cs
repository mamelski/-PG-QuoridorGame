using System;
using System.Collections.Generic;
using System.Text;
using Quoridor.DataContracts;

namespace Quoridor.Model
{
    public class InvitationListItem
    {
        public Invitation Invitation { get; set; }
        public Player Opponent { get; set; }

    }
}
