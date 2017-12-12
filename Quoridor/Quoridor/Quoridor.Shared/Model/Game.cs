using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Quoridor.DataContracts;
using Quoridor.Enums;
using Quoridor.Events;

namespace Quoridor.Model
{
    internal class Game
    {
        private readonly List<QuoridorPlayer> _players;
        private readonly Board board;
        private readonly QuoridorEventDispatcher dispatcher;
        private readonly QuoridorPlayer localPlayer;
        private readonly QuoridorWebService quoridorWebService;

        private QuoridorPlayer currentPlayer;
        private int currentPlayerIndex = -1;

        //public Action<bool> GameOverCallback;

        public Action<QuoridorPlayer> GameOverCallback;

        private bool gameStarted;
        //private GameType gameType;

        // private Player localUser;
        public Action<string> StatusCallback;

        private GameLocation gameLocation = GameLocation.InternetGame;
        private Game()
        {
            board = new Board(gameLocation);
            dispatcher = QuoridorEventDispatcher.getInstance();
            //dispatcher.OnMove += OpponentMove;
            quoridorWebService = dispatcher.QuoridorWebService;
            _players = new List<QuoridorPlayer>();
        }

        public Game(List<PlayerParameters> playersParameters) : this()
        {
            localPlayer = null;
            gameLocation = GameLocation.LocalGame;
            board = new Board(gameLocation);
            foreach (var playersParameter in playersParameters)
            {
                if(playersParameter.PlayerType==PlayerType.AI)
                    _players.Add(new AiPlayer(board,playersParameter));
                else if(playersParameter.PlayerType==PlayerType.Human)
                    _players.Add(new HumanPlayer(playersParameter));
            }

            SetNumberOfWallPerPlayer();

            board.PlacePlayers(_players);
        }

        public Game(GameType gameType, Player localUser, IEnumerable<Player> connectedUsers) : this()
        {
            switch (gameType)
            {
                case GameType.PlayerVsAi:
                    localPlayer = new HumanPlayer(null, localUser);
                    _players.Add(localPlayer);
                    _players.Add(new AiPlayer(board));
                    break;
                case GameType.PlayerVsPlayer:
                    localPlayer = new HumanPlayer(quoridorWebService, localUser);
                    _players.Add(localPlayer);
                    foreach (var user in connectedUsers)
                    {
                        if (user != localUser)
                            _players.Add(new HumanPlayer(quoridorWebService, user));
                    }
                    break;
                case GameType.AiVsAi:
                    localPlayer = null;
                    _players.Add(new AiPlayer(board));
                    _players.Add(new AiPlayer(board));
                    _players.Add(new AiPlayer(board));
                    _players.Add(new AiPlayer(board));
                    break;
            }

            SetPlayersOrderAndPositions();

            SetNumberOfWallPerPlayer();

            board.PlacePlayers(_players);
        }

        private void SetNumberOfWallPerPlayer()
        {
            foreach (var player in _players)
            {
                player.NumberOfWallsAvalaible = 20/_players.Count;
            }
        }

        private readonly Queue<Position> avalaibleStartingPositions = new Queue<Position>(new[] { Board.Bottom, Board.Top, Board.Right, Board.Left });

        private readonly Queue<Color> avalaibleColors = new Queue<Color>(new[] { Colors.LightBlue,Colors.Brown, Colors.LawnGreen,Colors.DeepPink, });

        private void SetPlayersOrderAndPositions()
        {
            _players.Sort((p1, p2) => p1.Id.CompareTo(p2.Id));

            if (localPlayer != null)
            {
                localPlayer.CurrentPosition = localPlayer.StartingPosition = avalaibleStartingPositions.Dequeue();
                localPlayer.PawnColor = avalaibleColors.Dequeue();
            }

            foreach (var player in _players)
                if (player != localPlayer)
                {
                    player.CurrentPosition = player.StartingPosition = avalaibleStartingPositions.Dequeue();
                    player.PawnColor = avalaibleColors.Dequeue();
                }
        }

        private bool WaitingForOponent => (currentPlayer != localPlayer && gameLocation == GameLocation.InternetGame) || (currentPlayer is AiPlayer && gameLocation == GameLocation.LocalGame);

        public TrulyObservableCollection<TrulyObservableCollection<BoardField>> BoardMatrix
        {
            get { return board.BoardMatrix; }
            set { board.BoardMatrix = value; }
        }

        public async void Start()
        {
            if (gameStarted) return;
            await NextTurn();
            gameStarted = true;
        }


        private async Task NextTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % _players.Count;
            currentPlayer = _players[currentPlayerIndex];
            StatusCallback?.Invoke($" Tura gracza: {currentPlayer.Name}");
            await NextMove();
        }


        private async Task NextMove()
        {
            if (!WaitingForOponent)
                return;

            var oponentMove = await currentPlayer.GetMove();


            board.MovePlayer(currentPlayer, oponentMove);


            if (board.GameOver)
            {
                GameOverCallback?.Invoke(board.Winner);
                return;
            }


            await NextTurn();
        }

        public async Task<bool> ClickBoard(Position position, Point clickPoint, Point centerOfElement)
        {
            if (WaitingForOponent)
                return false;

            var newMove = board.TryClick(position, clickPoint, centerOfElement, currentPlayer);
            if (newMove != null && newMove.IsWallPlacement)
                currentPlayer.NumberOfWallsAvalaible--;

            if (newMove == null)
                return false;

            var resu = await currentPlayer.SendMove(newMove);

            if (board.GameOver)
            {
                GameOverCallback?.Invoke(board.Winner);
                return true;
            }

            await NextTurn();
            return resu;
        }
    }
}