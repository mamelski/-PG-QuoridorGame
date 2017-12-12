using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MonoGame.Framework;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using QuoridorClient.Enums;
using QuoridorClient.Model;
using QuoridorClient.Screens;
using QuoridorClient.ServiceConnector;

namespace QuoridorClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly Game1 _game;
        private IServiceConnector _serviceConnector;
        private ObservableCollection<PlayerMessage> _messages;
        private ObservableCollection<Player> _players; 
        private int _myId;

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();
            _messages = new ObservableCollection<PlayerMessage>();
            //_messages.Add(new PlayerMessage() { Message = "Testowa wiadomosc"});
            _players = new ObservableCollection<Player>();
            //_players.Add(new Player() {Id = 1, Name = "Uzytkownik"});
            PlayersListView.ItemsSource = _players;
            MessagesListView.ItemsSource = _messages;

            // Create the game.

            _game = XamlGame<Game1>.Create(launchArguments, Window.Current.CoreWindow, this);
            _game.GamePage = this;
            _serviceConnector = SharedModel.GetConnector();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_serviceConnector.LogIn(LoginTextBox.Text, PasswordTextBox.Text)) {
                
                LoginPanel.Visibility = Visibility.Collapsed;
                ChatPanel.Visibility = Visibility.Collapsed;
                QuoridorService s1 = (QuoridorService) _serviceConnector;
                switch (LoginTextBox.Text) {
                    case "Mateusz":
                        s1.MyId = 4;
                        break;
                    case "Konrad":
                        s1.MyId = 2;
                        break;
                    default:
                        s1.MyId = 0;
                        break;
                }
                Session.Instance.GameState = GameState.Logged;
                Session.Instance.User = new Player() {Id = s1.MyId};
                //ScreenManager.Instance.SwitchScreen(new GameBoard(_game.GraphicsDevice));
            }
        }

        private void GamePage_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Screen tapped!");
        }

        private void LoginTextBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            LoginTextBox.Focus(FocusState.Keyboard);
        }

        private void PasswordTextBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            PasswordTextBox.Focus(FocusState.Keyboard);
        }

        public void UpdateMessages(List<PlayerMessage> msg)
        {
            foreach (var m in msg) {
                var name = from p in _players where p.Id == m.SenderId select p.Name;
                m.Message = name.FirstOrDefault() + ":" + m.Message;
                _messages.Add(m);
            } 
        }

        private void SendMessageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var player = PlayersListView.SelectedItem as Player;
            if (player == null) return;

            var message = new PlayerMessage
            {
                Message = MessageContentTextBox.Text,
                SenderId = _myId,
                ReceiverId = player.Id
            };

            _messages.Add(message);
            _serviceConnector.SendMessage(message.ReceiverId.ToString(), message.Message);
            MessageContentTextBox.Text = "";
        }

        private void MessageContentTextBox_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            MessageContentTextBox.Focus(FocusState.Keyboard);
        }

        public void UpdateConnectedPlayers(List<Player> connectedUsers)
        {
            foreach (var u in connectedUsers)
                _players.Add(u);
            SharedModel.ConnectedPlayers = new ConcurrentBag<Player>(connectedUsers);
        }

        private void Window_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Debug.WriteLine(e.Key);

            if (e.Key == Windows.System.VirtualKey.Left)
            {
                ScreenManager.Instance.CurrentScreen.HandleKeyboard(MoveDirection.Left);
            }
            if (e.Key == Windows.System.VirtualKey.Right)
            {
                ScreenManager.Instance.CurrentScreen.HandleKeyboard(MoveDirection.Right);
            }
            if (e.Key == Windows.System.VirtualKey.Up)
            {
                ScreenManager.Instance.CurrentScreen.HandleKeyboard(MoveDirection.Forward);
            }
            if (e.Key == Windows.System.VirtualKey.Down)
            {
                ScreenManager.Instance.CurrentScreen.HandleKeyboard(MoveDirection.Backward);
            }
        }
    }
}
