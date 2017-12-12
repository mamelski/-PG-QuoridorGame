using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Quoridor.Enums;
using Quoridor.Events;
using Quoridor.Model;

namespace Quoridor.UI
{
    using Quoridor.DataContracts;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChooseGameTypePage : Page
    {
        public ChooseGameTypePage() 
        {
            InitializeComponent();
        }

        Player player;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.player = e.Parameter as Player;
        }

        private void PlayerVsAiButton_OnClick(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(GamePage), new Dictionary<Type,object>() { { typeof(List<Player>), new List<Player>() {player,new Player()} }, { typeof(Player),player}, { typeof(GameType), GameType.PlayerVsAi } });
        }

        private void PlayerVsPlayerButton_OnClick(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof (AvailableUsersListPage), player);
            //            (Window.Current.Content as Frame).Navigate(typeof(GamePage), new Dictionary<Type, object>() { { typeof(Player), player }, { typeof(GameType), GameType.PlayerVsPlayer } });
        }

        private void AiVsAiButton_OnClick(object sender, RoutedEventArgs e)
        {

            (Window.Current.Content as Frame).Navigate(typeof(GamePage), new Dictionary<Type, object>() { { typeof(List<Player>), new List<Player>() { new Player(), new Player() } }, { typeof(Player), player }, { typeof(GameType), GameType.AiVsAi } });

        }

        private void LocalGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof (LocalGameLobbyPage));
        }
    }
}
