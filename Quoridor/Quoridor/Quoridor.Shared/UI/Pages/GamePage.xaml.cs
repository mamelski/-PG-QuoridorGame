using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Quoridor.DataContracts;
using Quoridor.Enums;
using Quoridor.Events;
using Quoridor.Model;

namespace Quoridor.UI
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        private Game game;
        private Player localUser;
       // private GameLocation gameLocation;

        public GamePage()
        {
            InitializeComponent();
        }

        public TrulyObservableCollection<TrulyObservableCollection<BoardField>> BoardMatrix
        {
            get { return game.BoardMatrix; }
            set { game.BoardMatrix = value; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dict = (Dictionary<Type, object>)e.Parameter;
            if (dict == null)
                throw new Exception("Wrong parameter, should be of type Dictionary<Type, object>");

            var playerParameters = dict.ContainsKey(typeof(List<PlayerParameters>)) ? dict[typeof(List<PlayerParameters>)] as List<PlayerParameters> : null;

            if (playerParameters == null)
            {
           //     gameLocation = GameLocation.InternetGame;
                localUser = dict[typeof (Player)] as Player;
                var connectedUsers = dict[typeof (List<Player>)] as List<Player>;
                var gameType = (GameType) dict[typeof (GameType)];


                game = new Game(gameType, localUser, connectedUsers)
                {
                    GameOverCallback = p =>
                    {
                        (Window.Current.Content as Frame).Navigate(typeof (GameOverPage),
                            new Dictionary<Type, object>
                            {
                                {typeof (Player), localUser},
                                {
                                    typeof (string),
                                    p.Id == localUser.Id && p.Name == localUser.Name
                                        ? $"Congratulations {localUser.Name}, you won!"
                                        : $"Sorry {localUser.Name}, you lost!"
                                }
                            });
                    }
                };
            }
            else
            {
             //   gameLocation = GameLocation.LocalGame;

                game = new Game(playerParameters)
                {
                    GameOverCallback = p =>
                    {
                        (Window.Current.Content as Frame).Navigate(typeof(GameOverPage),
                            new Dictionary<Type, object>
                            {
                                {typeof (Player), localUser},
                                {
                                    typeof (string),
                                    $"{p.Name} won!"
                                }
                            });
                    }
                };
            }

            game.StatusCallback = status => { StatusTextBox.Text = status; };
            game.Start();
        }

        private async void FieldTapped(object sender, TappedRoutedEventArgs e)
        {
            var rect = sender as Rectangle;
            if (rect == null)
                return;

            var position = GetPositionFromClick(e.GetPosition(GameBoardView));
            Debug.WriteLine($"position x={position.X}, y={position.Y}");
            var centerPoint = rect.TransformToVisual(GameBoardView).TransformPoint(new Point(0, 0));
            centerPoint = new Point(centerPoint.X + rect.Width / 2, centerPoint.Y + rect.Height / 2);

            var resu = await game.ClickBoard(position, e.GetPosition(GameBoardView),centerPoint);
        }

        private Position GetPositionFromClick(Point getPosition)
        {
            if (boardSizeProvider == null) {
                boardSizeProvider = new BoardSizeProvider();
            }

            var rect = new Rect(0, 0, boardSizeProvider.FieldSize, boardSizeProvider.FieldSize);

            for (var i = 0; i < 17; i++)
            {
                for (var j = 0; j < 17; j++)
                {
                    if (i%2 == 0 && j%2 == 0)
                    {
                        rect.X = j/2*(boardSizeProvider.FieldSize + boardSizeProvider.WallSize);
                        rect.Y = i/2*(boardSizeProvider.FieldSize + boardSizeProvider.WallSize);

                        rect.Width = boardSizeProvider.FieldSize;
                        rect.Height = boardSizeProvider.FieldSize;
                    }
                    else if (i%2 == 0)
                    {
                        rect.Y = i/2*(boardSizeProvider.FieldSize + boardSizeProvider.WallSize);
                        rect.Height = boardSizeProvider.FieldSize;

                        rect.X = j/2*boardSizeProvider.WallSize + (j/2 + 1)*boardSizeProvider.FieldSize;
                        rect.Width = boardSizeProvider.WallSize;
                    }
                    else if (j%2 == 0)
                    {
                        rect.X = j/2*(boardSizeProvider.FieldSize + boardSizeProvider.WallSize);
                        rect.Width = boardSizeProvider.FieldSize;

                        rect.Y = i/2*boardSizeProvider.WallSize + (i/2 + 1)*boardSizeProvider.FieldSize;
                        rect.Height = boardSizeProvider.WallSize;
                    }
                    else
                    {
                        rect.X = j/2*boardSizeProvider.WallSize + (j/2 + 1)*boardSizeProvider.FieldSize;
                        rect.Y = i/2*boardSizeProvider.WallSize + (i/2 + 1)*boardSizeProvider.FieldSize;

                        rect.Width = boardSizeProvider.WallSize;
                        rect.Height = boardSizeProvider.WallSize;
                    }

                    if (rect.Contains(getPosition))
                        return new Position {Y = i, X = j};
                }
            }
            //return new Position {Y = -1, X = -1};
             throw new ArgumentException();
        }
    }
}