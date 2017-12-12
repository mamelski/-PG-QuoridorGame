using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Quoridor.DataContracts;
using Quoridor.Enums;
using Quoridor.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Quoridor.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LocalGameLobbyPage : Page
    {
        public TrulyObservableCollection<PlayerParameters> Players { get; set; } 

        public PlayerStartingPosition[] PlayerStartingPositions { get; } =
            Enum.GetValues(typeof (PlayerStartingPosition)).Cast<PlayerStartingPosition>().ToArray();

        public PlayerType[] PlayerTypes { get; } =
           Enum.GetValues(typeof(PlayerType)).Cast<PlayerType>().ToArray();

        public LocalGameLobbyPage()
        {
            Players = new TrulyObservableCollection<PlayerParameters>
            {
                new PlayerParameters()
                {
                    Name = "Player 1",
                    PawnColor = Colors.Chartreuse,
                    StartingPosition = PlayerStartingPosition.Bottom,
                    PlayerType = PlayerType.Human
                },
                new PlayerParameters()
                {
                    Name = "Player 2",
                    PawnColor = Colors.Brown,
                    StartingPosition = PlayerStartingPosition.Top,
                    PlayerType = PlayerType.AI
                },
                new PlayerParameters()
                {
                    Name = "Player 3",
                    PawnColor = Colors.BlueViolet,
                    StartingPosition = PlayerStartingPosition.Left,
                    PlayerType = PlayerType.AI
                },
                new PlayerParameters()
                {
                    Name = "Player 4",
                    PawnColor = Colors.Magenta,
                    StartingPosition = PlayerStartingPosition.Right,
                    PlayerType = PlayerType.Human
                }
            };
            this.InitializeComponent();
            
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Players.Count < 4)
            {
                Players.Add(new PlayerParameters());
            }
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Players.Count > 2)
            {
                Players.RemoveAt(Players.Count-1);
            }
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ArePlayersValid())
            {
                (Window.Current.Content as Frame).Navigate(typeof (GamePage),
                    new Dictionary<Type, object>()
                    {
                        {typeof (List<PlayerParameters>), Players.ToList()},
                        {typeof (List<QuoridorPlayer>), null},
                        {typeof (Player), null},
                        {typeof (GameType), GameType.LocalGame}
                    });
            }
            else
            {
                new MessageDialog("Player should have unique and valid names, colors and starting positions").ShowAsync();
            }
        }

        private bool ArePlayersValid()
        {
            foreach (var p1 in Players)
                foreach (var p2 in Players)
                    if (!ReferenceEquals(p1, p2) &&
                        (p1.Name == p2.Name || p1.PawnColor == p2.PawnColor ||
                         p1.StartingPosition == p2.StartingPosition))
                        return false;

            foreach (var player in Players)
                if (string.IsNullOrEmpty(player.Name) || string.IsNullOrWhiteSpace(player.Name))
                    return false;
            return true;
        }
    }

}
