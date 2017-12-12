namespace QuoridorService
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Threading.Tasks;

    using global::Quoridor.DataContracts;

    /// <summary>
    /// The Quoridor interface.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IQuoridor
    {
        #region Test/Maintenance

        /// <summary>
        /// The get data.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [WebGet(UriTemplate = "/Test/{value}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string Test(string value);

        /// <summary>
        /// The clear all.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [WebGet(UriTemplate = "/ClearAll",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare)]
        bool ClearAll();

        #endregion
        
        #region SignUp/SignIn/LogOut

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
        [WebGet(UriTemplate = "/Register/{user}/{password}",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare)]
        Task<string> Register(string user, string password);

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
        /// The <see cref="Player"/>.
        /// </returns>
        [WebGet(UriTemplate = "/LogIn/{user}/{password}",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Player> LogIn(string user, string password);

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
        /// The <see cref="bool"/>.
        /// </returns>
        [WebGet(UriTemplate = "/LogOut/{user}/{password}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Task<bool> LogOut(string user, string password);

        #endregion

        #region MoveEvents

        /// <summary>
        /// The send board event.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json)]
        bool SendMove(Move move);

        /// <summary>
        /// The get board event.
        /// </summary>
        /// <param name="playerId">
        /// The player Id.
        /// </param>
        /// <returns>
        /// The <see cref="Move"/>.
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/GetMove/{playerId}",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json)]
        Move GetMove(string playerId);

        #endregion

        #region CreatingMatch

        /// <summary>
        /// The invite player.
        /// </summary>
        /// <param name="invitingPlayerId">
        /// The inviting player id.
        /// </param>
        /// <param name="invitedPlayerId">
        /// The invited player id.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/InvitePlayer/{invitingPlayerId}/{invitedPlayerId}",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json)]
        Invitation InvitePlayer(string invitingPlayerId, string invitedPlayerId);

        /// <summary>
        /// The check for invitation.
        /// </summary>
        /// <param name="playerId">
        /// The player id.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/CheckForInvitation/{playerId}",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json)]
        Invitation CheckForInvitation(string playerId);

        /// <summary>
        /// The delete invitation.
        /// </summary>
        /// <param name="invitation">
        /// The invitation.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/DeleteInvitation/{playerId}",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json)]
        bool DeleteInvitation(string playerId);

        /// <summary>
        /// The accept invitation.
        /// </summary>
        /// <param name="invitationGUID">
        /// The invitation guid.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/AcceptInvitation/{invitationGUID}",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json)]
        Invitation AcceptInvitation(string invitationGUID);

        /// <summary>
        /// The check invitation status.
        /// </summary>
        /// <param name="invitationGUID">
        /// The invitation GUID.
        /// </param>
        /// <returns>
        /// The <see cref="Invitation"/>.
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/CheckInvitationStatus/{invitationGUID}",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json)]
        Invitation CheckInvitationStatus(string invitationGUID);

        #endregion

        /// <summary>
        /// The log out all.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [WebGet(UriTemplate = "/LogOutAll",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Task<bool> LogOutAll();

        /// <summary>
        /// The get users online.
        /// </summary>
        /// <param name="playerIdString">
        /// The player id string.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        [OperationContract]
        [WebGet(UriTemplate = "/GetUsersOnline/{playerIdString}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        Task<List<Player>> GetUsersOnline(string playerIdString);
    }
}