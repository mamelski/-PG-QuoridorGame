//#define _USE_LOCAL_SERVICE
namespace Quoridor
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using Quoridor.DataContracts;
    using Quoridor.Events;

    /// <summary>
    /// The quoridor web service.
    /// </summary>
    public class QuoridorWebService
    {
        /// <summary>
        /// The quoridor service.
        /// Local path: http://localhost:37629/Quoridor.svc
        /// </summary>
#if _USE_LOCAL_SERVICE
#warning Using local service, remember to start local Azure simulator
        private readonly ServiceWrapper quoridorService = new ServiceWrapper(@"http://localhost:37629/Quoridor.svc");
#else
        private readonly ServiceWrapper quoridorService = new ServiceWrapper(@"http://quoridor.cloudapp.net/Quoridor.svc");
#endif
        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// The log in.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<LogInEvent> LogIn(string username, string password)
        {
            var player = await this.quoridorService.CallGetMethod<Player>("LogIn", username, password);

            var logged = player != null && player.Id != Guid.Empty;
            this.PlayerId = player.Id;

            return new LogInEvent { IsLogged = logged, Me = player };
        }

        /// <summary>
        /// The get messages.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<List<PlayerMessage>> GetMessages()
        {
            return await this.quoridorService.CallGetMethod<List<PlayerMessage>>("GetMessages", this.PlayerId.ToString());
        }

        /// <summary>
        /// The get connected users.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<List<Player>> GetConnectedUsers()
        {
            return await this.quoridorService.CallGetMethod<List<Player>>("GetUsersOnline", this.PlayerId.ToString());
        }

#region GetMove

        /// <summary>
        /// The send move.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> SendMove(Move move)
        {
            return await this.quoridorService.CallPostMethod("SendMove", move);
        }

        /// <summary>
        /// The get move.
        /// </summary>
        /// <param name="playerId">
        /// The player id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Move> GetMove(Guid playerId)
        {
            return await this.quoridorService.CallGetMethod<Move>("GetMove", playerId.ToString());
        }

#endregion GetMove

#region CheckAwaitingInvitation

        /// <summary>
        /// The invite player.
        /// </summary>
        /// <param name="invitedPlayerId">
        /// The invited player id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Invitation> InvitePlayer(string invitedPlayerId)
        {
            return await this.quoridorService.CallGetMethod<Invitation>("InvitePlayer", this.PlayerId.ToString(), invitedPlayerId);
        }

        /// <summary>
        /// The check for invitation.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Invitation> CheckForInvitation()
        {
            return await this.quoridorService.CallGetMethod<Invitation>("CheckForInvitation", this.PlayerId.ToString());
        }

        /// <summary>
        /// The accept invitation.
        /// </summary>
        /// <param name="invitation">
        /// The invitation.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Invitation> AcceptInvitation(Invitation invitation)
        {
          return await this.quoridorService.CallGetMethod<Invitation>("AcceptInvitation", invitation.InvitationGuidString);
        }

        /// <summary>
        /// The check invitation status.
        /// </summary>
        /// <param name="invitation">
        /// The invitation.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Invitation> CheckInvitationStatus(Invitation invitation)
        {
            return await this.quoridorService.CallGetMethod<Invitation>("CheckInvitationStatus", invitation.InvitationGuidString);
        }

#endregion CheckAwaitingInvitation

        public async Task<String> Register(String username, String password)
        {
            return await quoridorService.CallGetMethod<String>("Register", username, password);
        }
    }
}