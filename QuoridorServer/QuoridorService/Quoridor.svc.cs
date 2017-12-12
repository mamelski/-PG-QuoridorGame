namespace QuoridorService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.MobileServices;

    using global::Quoridor.DataContracts;

    /// <summary>
    /// The quoridor.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class Quoridor : IQuoridor
    {
        /// <summary>
        /// The data base service.
        /// </summary>
        private static readonly MobileServiceClient DataBaseService = new MobileServiceClient("http://siatkostat.azure-mobile.net/", "SWEeIPTQrncilpfBbUbtmcIvgtDVOM70");

        /// <summary>
        /// The player table.
        /// </summary>
        private readonly IMobileServiceTable<QuoridorPlayer> playerTable = DataBaseService.GetTable<QuoridorPlayer>();

        /// <summary>
        /// The last move by player.
        /// </summary>
        private readonly Dictionary<Guid, Move> lastMoveByPlayer = new Dictionary<Guid, Move>();

        /// <summary>
        /// The invitations.
        /// </summary>
        private Dictionary<Guid, Invitation> invitations = new Dictionary<Guid, Invitation>();

        /// <summary>
        /// The test.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string Test(string value)
        {
            return string.Format("Wpisałeś : {0}", value);
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public async Task<string> Register(string user, string password)
        {
            var newUser = new QuoridorPlayer { Login = user, Password = password };

            try
            {
                var playersCollection = await this.playerTable.ToCollectionAsync();
                if (playersCollection.Any(player => player.Login == user))
                {
                    return "User with given name already exists";
                }

                await this.playerTable.InsertAsync(newUser);

                this.invitations.Add(newUser.Id, null);
            }
            catch (Exception)
            {
                return "Unknown error";
            }

            return "Success";
        }

        /// <summary>
        /// The log in.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Player> LogIn(string user, string password)
        {
            try
            {
                var playersCollection = await this.playerTable.ToCollectionAsync();
                var playerInDataBase = playersCollection.Single(p => p.Login == user && p.Password == password);
                playerInDataBase.IsOnline = true;
                await this.playerTable.UpdateAsync(playerInDataBase);
                return new Player { Name = playerInDataBase.Login, Id = playerInDataBase.Id };
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The log out.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> LogOut(string user, string password)
        {
            try
            {
                var playersCollection = await this.playerTable.ToCollectionAsync();
                var playerInDataBase = playersCollection.Single(p => p.Login == user && p.Password == password);
                playerInDataBase.IsOnline = false;
                await this.playerTable.UpdateAsync(playerInDataBase);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The clear all.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ClearAll()
        {
            var newInvitations = this.invitations.Keys.ToDictionary<Guid, Guid, Invitation>(user => user, user => null);
            this.invitations = newInvitations;

            foreach (var user in this.lastMoveByPlayer.Keys)
            {
                this.lastMoveByPlayer[user] = null;
            }

            return true;
         }

        /// <summary>
        /// The log out all.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> LogOutAll()
        {
            try
            {
                var playersCollection = await this.playerTable.ToCollectionAsync();

                foreach (var player in playersCollection)
                {
                    player.IsOnline = false;
                    await this.playerTable.UpdateAsync(player);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The get users online.
        /// </summary>
        /// <param name="playerIdString">
        /// The player id string.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<List<Player>> GetUsersOnline(string playerIdString)
        {
            var onlinePlayers = new List<Player>();
            
            Guid playerId;
            Guid.TryParse(playerIdString, out playerId);

            try
            {
                var playersCollection = await this.playerTable.ToCollectionAsync();

                foreach (var player in playersCollection)
                {
                    if (player.IsOnline && player.Id != playerId)
                    {
                        onlinePlayers.Add(new Player { Id = player.Id, Name = player.Login });
                    }
                }

                return onlinePlayers;
            }
            catch (Exception)
            {
                return onlinePlayers;
            }
        }

        #region GameEvents

        /// <summary>
        /// The get move.
        /// </summary>
        /// <param name="playerIdString">
        /// The player id string.
        /// </param>
        /// <returns>
        /// The <see cref="Move"/>.
        /// </returns>
        public Move GetMove(string playerIdString)
        {
            var playerId = Guid.Parse(playerIdString);

            Move move = null;

            if (this.lastMoveByPlayer.ContainsKey(playerId))
            {
                move = this.lastMoveByPlayer[playerId];
                this.lastMoveByPlayer[playerId] = null;
            }

            return move;
        }

        /// <summary>
        /// The send move.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendMove(Move move)
        {
            this.lastMoveByPlayer[move.Player.Id] = move;
            return true;
        }

        #endregion

        #region CreatingMatch

        /// <summary>
        /// The invite player.
        /// </summary>
        /// <param name="invitingPlayerIdString">
        /// The inviting player id string.
        /// </param>
        /// <param name="invitedPlayerIdString">
        /// The invited player id string.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        public Invitation InvitePlayer(string invitingPlayerIdString, string invitedPlayerIdString)
        {
            var invitingPlayerId = Guid.Parse(invitingPlayerIdString);
            var invitedPlayerId = Guid.Parse(invitedPlayerIdString);

            var newInvitation = new Invitation
                                    {
                                        InvitationGuidString = Guid.NewGuid().ToString(),
                                        InvitedPlayerId = invitedPlayerId,
                                        InvitingPlayerId = invitingPlayerId
                                    };

            this.invitations[invitedPlayerId] = newInvitation;
            this.invitations[invitingPlayerId] = newInvitation;

            return newInvitation;
        }

        /// <summary>
        /// The check for invitation.
        /// </summary>
        /// <param name="playerIdString">
        /// The player id string.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        public Invitation CheckForInvitation(string playerIdString)
        {
            var playerId = Guid.Parse(playerIdString);
            return this.invitations[playerId];
        }

        /// <summary>
        /// The delete invitation.
        /// </summary>
        /// <param name="playerId">
        /// The player id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteInvitation(string playerId)
        {
            var playerGuid = Guid.Parse(playerId);
            this.invitations[playerGuid] = null;
            return true;
        }

        /// <summary>
        /// The accept invitation.
        /// </summary>
        /// <param name="invitationGUID">
        /// The invitation guid.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        public Invitation AcceptInvitation(string invitationGUID)
        {
            var invitation = this.invitations.Values.First(inv => inv.InvitationGuidString == invitationGUID);
            invitation.MatchGuidString = Guid.NewGuid().ToString();

            return invitation;
        }

        /// <summary>
        /// The check invitation status.
        /// </summary>
        /// <param name="invitationGUID">
        /// The invitation guid.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        public Invitation CheckInvitationStatus(string invitationGUID)
        {
            var invitation = this.invitations.Values.First(inv => inv.InvitationGuidString == invitationGUID);

            return invitation;
        }

        #endregion
    }
}
