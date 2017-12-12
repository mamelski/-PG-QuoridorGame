using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Quoridor.DataContracts;
using Quoridor.Enums;
using Quoridor.Events;
using Quoridor.Model;

namespace Quoridor.UI
{
    /// <summary>
    ///     List of available users.
    /// </summary>
    public sealed partial class AvailableUsersListPage : Page
    {
        private readonly QuoridorEventDispatcher _dispatcher;
        private readonly QuoridorWebService quoridorWebService;

        private List<Invitation> _myInvitations = new List<Invitation>();
        private List<InvitationListItem> _gameList = new List<InvitationListItem>();

        public AvailableUsersListPage()
        {
            InitializeComponent();
            _dispatcher = QuoridorEventDispatcher.getInstance();
            quoridorWebService = _dispatcher.QuoridorWebService;
//            var task = DeserializeInvitations().Result;

            _dispatcher.AddRequest(new EventRequest { RequestType = EventRequestType.CheckAwaitingInvitation });
            _dispatcher.AddRequest(new EventRequest { RequestType = EventRequestType.GetLoggedUsers });

            if (_myInvitations.Count > 0) {
                _dispatcher.AddRequest(new EventRequest { RequestType = EventRequestType.CheckAcceptedInvitation, RequestObject = _myInvitations.FirstOrDefault() });
            }
        }

        private Player localPlayer;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            localPlayer = (Player)e.Parameter;
            _dispatcher.SetLoggedPlayer(localPlayer);
            RegisterForEvents();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            UnregisterForEvents();
        }

        private void RegisterForEvents()
        {
            _dispatcher.LoggedUsersRefreshed += LoggedUserRefreshed;
            _dispatcher.InvitationReceived += InvitationReceived;
            _dispatcher.StartGame += DispatcherOnStartGame;
        }

        private void UnregisterForEvents()
        {
            _dispatcher.LoggedUsersRefreshed -= LoggedUserRefreshed;
            _dispatcher.InvitationReceived -= InvitationReceived;
            _dispatcher.StartGame -= DispatcherOnStartGame;
        }

        private void DispatcherOnStartGame(object sender, InvitationHandshake invitationHandshake)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            UsersListView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Guid opponentId = (_myInvitations.Any(inv => inv.InvitationGuidString == invitationHandshake.Invitation.InvitationGuidString))
                    ? invitationHandshake.Invitation.InvitedPlayerId
                    : invitationHandshake.Invitation.InvitingPlayerId;

                try {
                    var opponent = GetPlayerById(opponentId);
                    if (String.IsNullOrEmpty(opponent.Name)) {
                        opponent.Name = invitationHandshake.Invitation.InvitationGuidString;
                    }

                    if (_gameList.All(inv => inv.Invitation.InvitationGuidString != invitationHandshake.Invitation.InvitationGuidString)) {
                        _gameList.Add(new InvitationListItem {Invitation = invitationHandshake.Invitation, Opponent = opponent});
                        InvitationsListView.ItemsSource = _gameList;
                    }

                    //                    var navigationParams = new Dictionary<Type, object>() {
                    //                        {typeof (Player), opponent},
                    //                        {typeof (GameType), GameType.PlayerVsPlayer}
                    //                    };
                    //                    (Window.Current.Content as Frame).Navigate(typeof(GamePage), navigationParams);
                } catch (NullReferenceException ex) {
                    Debug.WriteLine("NPE caught at DispatcherOnStartGame: " + ex.Message);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private Player GetPlayerById(Guid opponentId)
        {
            Player opponent = null;

            List<Player> players = (List<Player>) UsersListView.ItemsSource;  
            if (players != null) {
                opponent = (from player in players where player.Id == opponentId select player).FirstOrDefault();
            }
            opponent = opponent ?? new Player { Id = opponentId };

            return opponent;
        }

        private void InvitationReceived(object sender, InvitationHandshake e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            UsersListView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                try {
                    _dispatcher.AddRequest(new EventRequest {RequestType = EventRequestType.AcceptInvitation, RequestObject = e.Invitation });
                } catch (NullReferenceException ex) {
                    Debug.WriteLine("NPE caught at InvitationReceived:" + ex.Message);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void LoggedUserRefreshed(object sender, LoggedUsersRefreshed e)
        {
            var users = new List<Player>(e.LoggedUsers);
            UsersListView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UsersListView.ItemsSource = users);
        }

        private void InvitePlayerButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsersListView.SelectedItem != null)
            {
                var chosenPlayer = UsersListView.SelectedItem as Player;
                if (chosenPlayer != null) {
                    Invite(chosenPlayer.Id);
                }
            }
        }

        private async void Invite(Guid playerId)
        {
            var invitation = await _dispatcher.Invite(playerId);
            await UsersListView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _myInvitations.Add(invitation);
                SerializeInvitations();
            });
        }
        private Random random = new Random();
        private string invitationFileName;

        private async void SerializeInvitations()
        {
            invitationFileName = $"inv{random.Next()}.json";
            StorageFolder localFolder = ApplicationData.Current.TemporaryFolder;
            StorageFile invFile = await localFolder.CreateFileAsync(invitationFileName, CreationCollisionOption.ReplaceExisting);

            using (IRandomAccessStream textStream = await invFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (DataWriter textWriter = new DataWriter(textStream)) {
                    String json = JsonConvert.SerializeObject(_myInvitations);
                    textWriter.WriteString(json);
                    await textWriter.StoreAsync();
                }
            }
        }

        //TODO: Ogarnąć żeby działało - na razie z jakigoś powodu zawisa na sprawdzeniu pliku/zawartości folderu
        private async Task<List<Invitation>> DeserializeInvitations()
        {
            List<Invitation> invitations = new List<Invitation>();
            StorageFolder localFolder = ApplicationData.Current.TemporaryFolder;

            try {
                var files = await localFolder.GetFilesAsync();
                if(files.Any(file => file.Name == invitationFileName)) {
                    StorageFile invFile = await localFolder.GetFileAsync(invitationFileName);

                    if (invFile != null) {
                        using (IRandomAccessStream textStream = await invFile.OpenReadAsync()) {
                            using (DataReader reader = new DataReader(textStream)) {
                                uint textLength = (uint) textStream.Size;
                                await reader.LoadAsync(textLength);
                                String json = reader.ReadString(textLength);
                                invitations = JsonConvert.DeserializeObject<List<Invitation>>(json);
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                //File not found
            }

            return invitations;
        } 

        private void RefreshListButton_OnClick(object sender, RoutedEventArgs e)
        {
            _dispatcher.AddRequest(new EventRequest { RequestType = EventRequestType.GetLoggedUsers });
        }

        private void StartGameButton_OnClickButton(object sender, RoutedEventArgs e)
        {
            if (InvitationsListView.SelectedItem != null)
            {
                var chosenGame = InvitationsListView.SelectedItem as InvitationListItem;
                Player opponent = chosenGame.Opponent;

                var players = new List<Player>() {localPlayer, opponent};

                var navigationParams = new Dictionary<Type, object>() {
                        {typeof (Player), localPlayer},
                        {typeof (List<Player>), players},
                        {typeof (GameType), GameType.PlayerVsPlayer}
                    };
                (Window.Current.Content as Frame).Navigate(typeof(GamePage), navigationParams);
            }
        }
    }
}