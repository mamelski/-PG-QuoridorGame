using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Quoridor.DataContracts;

namespace Quoridor.Events
{
    public class QuoridorEventDispatcher : WebEventDispatcher
    {
        private static QuoridorEventDispatcher _dispatcher;
        private Task _gameEventsReceiverTask;
        private Task _getUsersTask;
        private Task _invitationHandlerTask;

        private Player _loggedPlayer;
        private Task _messageReceiverTask;

        private const int NextCallDelay = 2000;

        private QuoridorEventDispatcher()
        {
            QuoridorWebService = new QuoridorWebService();
            _loggedPlayer = new Player {Id = Guid.Empty};
        }

        public QuoridorWebService QuoridorWebService { get; }

        public event EventHandler<LogInEvent> LoggedIn;
        public event EventHandler<MessagesInboundEvent> MessagesInbound;
        public event EventHandler<LoggedUsersRefreshed> LoggedUsersRefreshed;
        public event EventHandler<InvitationHandshake> InvitationReceived;
        public event EventHandler<InvitationHandshake> StartGame;
        public event EventHandler<MoveEventArgs> OnMove;

        public static QuoridorEventDispatcher getInstance()
        {
            if (_dispatcher == null)
            {
                _dispatcher = new QuoridorEventDispatcher();
            }

            return _dispatcher;
        }

        public void SetLoggedPlayer(Player player)
        {
            _loggedPlayer = player;
            QuoridorWebService.PlayerId = player.Id;
        }

        protected override void DispatchEventRequest(EventRequest eventRequest)
        {
            switch (eventRequest.RequestType)
            {
                case EventRequestType.LogIn:
                    HandleLogInRequest(eventRequest);
                    break;
//                case EventRequestType.CheckMessages:
//                    HandleCheckMessagesRequest();
//                    break;
                case EventRequestType.GetLoggedUsers:
                    HandleGetLoggedUsers();
                    break;
                case EventRequestType.CheckGameEvents:
                    HandleCheckGameEvents();
                    break;
                case EventRequestType.SendMessage:
                    HandleSendMessage(eventRequest);
                    break;
                case EventRequestType.CheckAwaitingInvitation:
                    HandleAwaitingInvitations(eventRequest);
                    break;
                case EventRequestType.Invite:
                    HandleSendingInvitation(eventRequest);
                    break;
                case EventRequestType.AcceptInvitation:
                    HandleAcceptInvitation(eventRequest);
                    break;
                case EventRequestType.CheckAcceptedInvitation:
                    HandleAcceptedInvitation(eventRequest);
                    break;
                case EventRequestType.SendGameEvent:
                    break;
            }
        }

        private async void HandleAcceptedInvitation(EventRequest eventRequest)
        {
            try {
                Invitation inv = (Invitation) eventRequest.RequestObject;
                Invitation recievedInv = await QuoridorWebService.CheckInvitationStatus(inv);
                if (recievedInv != null && recievedInv.MatchGuidString != null) {
                    var handler = StartGame;
                    handler?.Invoke(this, new InvitationHandshake {Invitation = recievedInv, EventMessage = ""});
                } else if (Run) {
                    await Task.Delay(NextCallDelay);
                    AddRequest(new EventRequest {RequestType = EventRequestType.CheckAcceptedInvitation, RequestObject = inv});
                }
            } catch (Exception ex) {
                Debug.WriteLine("Failed to handle accepted invitation:" + ex.Message);
            }
        }

        private async void HandleAcceptInvitation(EventRequest eventRequest)
        {
            try {
                Invitation inv = (Invitation) eventRequest.RequestObject;
                if (inv != null) {
                    inv = await QuoridorWebService.AcceptInvitation(inv);
                    var handler = StartGame;
                    handler?.Invoke(this, new InvitationHandshake {Invitation = inv, EventMessage = ""});
                }
            } catch (Exception ex) {
                Debug.WriteLine("Failed to handle invitation acceptance:" + ex.Message);
            }
        }

        private async void HandleSendingInvitation(EventRequest eventRequest)
        {
            try {
                String invitedPlayerId = (String) eventRequest.RequestObject;
                if (invitedPlayerId != null) {
                    Invitation inv = await QuoridorWebService.InvitePlayer(invitedPlayerId);
                    await Task.Delay(NextCallDelay);
                    AddRequest(new EventRequest {RequestType = EventRequestType.CheckAcceptedInvitation, RequestObject = inv});
                }
            } catch (Exception ex) {
                Debug.WriteLine("Failed to send invitation:" + ex.Message);
            }
        }

        private async void HandleAwaitingInvitations(EventRequest eventRequest)
        {
            try {
                var inv = await QuoridorWebService.CheckForInvitation();
                var handler = InvitationReceived;
                if (inv != null) {
                    handler?.Invoke(this, new InvitationHandshake {EventMessage = "", Invitation = inv});
                }

                if (Run) {
                    await Task.Delay(NextCallDelay);
                    AddRequest(new EventRequest {RequestType = EventRequestType.CheckAwaitingInvitation});
                }
            } catch (Exception ex) {
                Debug.WriteLine("Failed to handle awaiting invitation:" + ex.Message);
            }
        }

        private void HandleSendMessage(EventRequest eventRequest)
        {
            var requestObject = (Dictionary<string, string>) eventRequest.RequestObject;
           // QuoridorWebService.SendMessage(requestObject["userId"], requestObject["message"]);
        }
        
        //TODO
        private async void HandleCheckGameEvents()
        {
            var move = await QuoridorWebService.GetMove(_loggedPlayer.Id);

            var handler = OnMove;
            if (handler != null)
            {
                handler(this, new MoveEventArgs {Move = move});
            }

            if (Run)
            {
                await Task.Delay(NextCallDelay);
                AddRequest(new EventRequest {RequestType = EventRequestType.CheckGameEvents});
            }
        }

        private async void HandleGetLoggedUsers()
        {
            var players = await QuoridorWebService.GetConnectedUsers();
            var handler = LoggedUsersRefreshed;
            if (handler != null)
            {
                handler(this, new LoggedUsersRefreshed {LoggedUsers = players});
            }
        }

        private async void HandleCheckMessagesRequest()
        {
            var messages = await QuoridorWebService.GetMessages();//sometimes throws An exception of type 'System.Threading.Tasks.TaskCanceledException' occurred in mscorlib.dll but was not handled in user code
            //Additional information: A task was canceled.
            if (messages.Count > 0)
            {
                var handler = MessagesInbound;
                if (handler != null)
                {
                    handler(this, new MessagesInboundEvent {Messages = messages});
                }
            }

            if (Run)
            {
                await Task.Delay(NextCallDelay);
                AddRequest(new EventRequest {RequestType = EventRequestType.CheckMessages});
            }
        }

        private async void HandleLogInRequest(EventRequest eventRequest)
        {
            var credentials = (Dictionary<string, string>) eventRequest.RequestObject;
            var logInEvent = await QuoridorWebService.LogIn(credentials["username"], credentials["password"]);
            var handler = LoggedIn;
            if (handler != null)
            {
                handler(this, logInEvent);
                if (logInEvent.IsLogged)
                {
                    _loggedPlayer = logInEvent.Me;

//                    _messageReceiverTask = new Task(CheckForMessages);
//                    _messageReceiverTask.Start();

                    AddRequest(new EventRequest {RequestType = EventRequestType.CheckMessages});
                    AddRequest(new EventRequest {RequestType = EventRequestType.CheckGameEvents});

//                    _getUsersTask = new Task(GetLoggedUsers);
//                    _getUsersTask.Start();

                    _invitationHandlerTask = new Task(InvitationReceiver);
                }
            }
        }

        private async void InvitationReceiver()
        {
            while (Run)
            {
                AddRequest(new EventRequest {RequestType = EventRequestType.CheckAwaitingInvitation});
                await Task.Delay(NextCallDelay);
            }
        }

        private async void CheckForMessages()
        {
            while (Run)
            {
                AddRequest(new EventRequest {RequestType = EventRequestType.CheckMessages});
                await Task.Delay(NextCallDelay);
                AddRequest(new EventRequest {RequestType = EventRequestType.CheckGameEvents});
                await Task.Delay(NextCallDelay);
            }
        }

        private async void GetLoggedUsers()
        {
            while (Run)
            {
                AddRequest(new EventRequest {RequestType = EventRequestType.GetLoggedUsers});
                await Task.Delay(NextCallDelay);
            }
        }

        public void LogIn(string username, string password)
        {
            if (!IsRunning()) {
                Start();
            }

            var credentials = new Dictionary<string, string>();
            credentials.Add("username", username);
            credentials.Add("password", password);
            var request = new EventRequest
            {
                RequestType = EventRequestType.LogIn,
                RequestObject = credentials
            };
            AddRequest(request);
        }

        public void LogOut()
        {
            Stop();
        }

        public void SendMessage(int userId, string message)
        {
            var requestObject = new Dictionary<string, string> {{"userId", userId.ToString()}, {"message", message}};
            var request = new EventRequest
            {
                RequestType = EventRequestType.SendMessage,
                RequestObject = requestObject
            };
            AddRequest(request);
        }

        public async Task<Invitation> Invite(Guid userId)
        {
            Invitation inv = await QuoridorWebService.InvitePlayer(userId.ToString());
            AddRequest(new EventRequest { RequestType = EventRequestType.CheckAcceptedInvitation, RequestObject = inv });
            return inv;
        }

        public Invitation GetInvitationStatus(Invitation invitation)
        {
            return QuoridorWebService.CheckInvitationStatus(invitation).Result;
        }
    }
}