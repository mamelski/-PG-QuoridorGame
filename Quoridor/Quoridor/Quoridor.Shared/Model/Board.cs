//#define _USE_ADDITIONAL_VALIDATION

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Quoridor.DataContracts;
using Quoridor.Enums;

namespace Quoridor.Model
{
    public class Board
    {
        public const int BoardSize = 16;
       // private QuoridorPlayer _localPlayer;
        private GameLocation gameLocation;
        private bool _playersPlaced;

        public Board(IEnumerable<QuoridorPlayer> players, GameLocation gameLocation) : this(gameLocation)
        {
            Players = players.ToList();
            //_localPlayer = localPlayer;

            foreach (var player in Players)
            {
               // if (player != localPlayer)
                    this[player.StartingPosition] = BoardElementType.Player;
            }

         //   this[localPlayer.StartingPosition] = BoardElementType.Player;
        }

        public Board(GameLocation gameLocation)
        {
            this.gameLocation = gameLocation;
            BoardMatrix = new TrulyObservableCollection<TrulyObservableCollection<BoardField>>();
            for (var i = 0; i <= BoardSize; i++)
            {
                BoardMatrix.Add(new TrulyObservableCollection<BoardField>());
                for (var j = 0; j <= BoardSize; j++)
                {
                    if (i%2 == 0 && j%2 == 0)
                        BoardMatrix[i].Add(new BoardField
                        {
                            Position = new Position {Y = i, X = j},
                            FieldType = BoardElementType.Empty
                        });
                    else
                        BoardMatrix[i].Add(new BoardField
                        {
                            Position = new Position {Y = i, X = j},
                            FieldType = BoardElementType.EmptyForWall
                        });
                }
            }
        }

        public List<QuoridorPlayer> Players { get; private set; }

        public IEnumerable<Position> PlayersPositions
        {
            get { return Players.Select(p => p.CurrentPosition); }
        }

        public BoardElementType this[Position position]
        {
            get { return BoardMatrix[position.Y][position.X].FieldType; }
            set { BoardMatrix[position.Y][position.X].FieldType = value; }
        }


        public bool GameOver => Players.Any(p => p.HasWon);

        public QuoridorPlayer Winner => Players.FirstOrDefault(p => p.HasWon);

        public TrulyObservableCollection<TrulyObservableCollection<BoardField>> BoardMatrix { get; set; }

        public BoardArray GetBoardArray()
        {
            var boardArray = new BoardArray();

            for (var y = 0; y < BoardMatrix.Count; y++)
            {
                for (var x = 0; x < BoardMatrix[y].Count; x++)
                {
                    boardArray[y, x] = BoardMatrix[y][x].FieldType;
                }
            }
            return boardArray;
        }

        public void PlacePlayers(IEnumerable<QuoridorPlayer> players)
        {
            if (_playersPlaced)
                throw new Exception("You shouldnt place players more than one time");

            Players = players.ToList();
           // _localPlayer = localPlayer;

            foreach (var player in Players)
            {
              //  if (player != localPlayer)
                {
                    this[player.StartingPosition] = BoardElementType.Player;
                    BoardMatrix[player.CurrentPosition.Y][player.CurrentPosition.X].Player = player;
                }
            }

          //  if (localPlayer != null)
            {
          //      this[localPlayer.StartingPosition] = BoardElementType.Player;
          //      BoardMatrix[localPlayer.CurrentPosition.Y][localPlayer.CurrentPosition.X].Player = localPlayer;
            }

            _playersPlaced = true;
        }


        public Move TryClick(Position position, Point clickPoint, Point centerOfElement, QuoridorPlayer currentPlayer)
        {
            switch (this[position])
            {
                case BoardElementType.Empty:
                    return TryMove(position, currentPlayer);
                case BoardElementType.EmptyForWall:
                    return TryPlaceWall(position, clickPoint, centerOfElement, currentPlayer);
                default:
                    return null;
            }
        }

        private Move TryPlaceWall(Position position, Point clickPoint, Point centerOfElement, QuoridorPlayer currentPlayer)
        {
            if (this[position] != BoardElementType.EmptyForWall || currentPlayer.NumberOfWallsAvalaible <= 0)
                return null;
            Position wall1Position = null; // = new Position(1, 0);
            Position wall2Position = null; // = new Position(2, 0);
            var boardArray = GetBoardArray();

            if (BoardMatrix[position.Y][position.X].IsHorizontalWall)
            {
                wall1Position = new Position(1, 0);
                wall2Position = new Position(2, 0);
                if (clickPoint.X <= centerOfElement.X)
                {
                    wall1Position = -wall1Position;
                    wall2Position = -wall2Position;
                }
            }
            else if (BoardMatrix[position.Y][position.X].IsVerticalWall)
            {
                wall1Position = new Position(0, 1);
                wall2Position = new Position(0, 2);
                if (clickPoint.Y <= centerOfElement.Y)
                {
                    wall1Position = -wall1Position;
                    wall2Position = -wall2Position;
                }
            }
            else if (BoardMatrix[position.Y][position.X].IsMicroWall)
            {
                //TODO: handle this rare case when user clicks exactly on micro wall
                return null;
            }

            if (WallValidator.AreValid(boardArray, Players, currentPlayer, position, position + wall2Position,
                position + wall1Position))
            {
              //  if (changeAllowed)
                {
                    PlaceWall(position);
                    PlaceWall(position + wall1Position);
                    PlaceWall(position + wall2Position);
                }
                return new Move
                {
                    IsWallPlacement = true,
                    WallPlacementPositions = new[] {position, position + wall2Position, position + wall1Position}
                };
            }
            if (WallValidator.AreValid(boardArray, Players, currentPlayer, position, position - wall2Position,
                position - wall1Position))
            {
              //  if (changeAllowed)
                {
                    PlaceWall(position);
                    PlaceWall(position - wall1Position);
                    PlaceWall(position - wall2Position);
                }
                return new Move
                {
                    IsWallPlacement = true,
                    WallPlacementPositions = new[] {position, position - wall2Position, position - wall1Position}
                };
            }
            return null;
        }

        private Move TryMove(Position position, QuoridorPlayer currentPlayer)
        {
            if (MoveValidator.IsValid(GetBoardArray(), currentPlayer.CurrentPosition,
                Players.Select(p => p.CurrentPosition), position))
            {
                //if (changeAllowed)
                {
                    MovePlayer(currentPlayer,position);
                }
                return new Move {Destination = position};
            }
            return null;
        }


        public void MovePlayer(QuoridorPlayer player, Move move)
        {
            if (!Players.Contains(player))
                throw new ArgumentException();

            if (move.IsMove)
            {
                var position = TranslatePlayerPosition(player,move.Destination);
#if _USE_ADDITIONAL_VALIDATION
                if (!MoveValidator.IsValid(GetBoardArray(), player.CurrentPosition,Players.Select(p => p.CurrentPosition), position))
                    //this is kind of second time validation - we dont neceserry need it in production
                    throw new ArgumentException();
#endif
                var playerBoardElement = this[player.CurrentPosition];

                this[player.CurrentPosition] = BoardElementType.Empty;
                BoardMatrix[player.CurrentPosition.Y][player.CurrentPosition.X].Player = null;

                player.CurrentPosition = position;

                this[player.CurrentPosition] = playerBoardElement;
                BoardMatrix[player.CurrentPosition.Y][player.CurrentPosition.X].Player = player;
            }
            else if (move.IsWallPlacement)
            {
                var positions =
                    move.WallPlacementPositions.Select(p => TranslatePlayerPosition(player,p));
#if _USE_ADDITIONAL_VALIDATION
                if (!WallValidator.AreValid(GetBoardArray(), Players, player, positions.ToArray()))
                    //this is kind of second time validation - we dont neceserry need it in production
                    throw new ArgumentException();
#endif
                foreach (var position in positions)
                {
                    this[position] = BoardElementType.Wall;
                }
            }
            else
                throw new ArgumentException();
        }

        private void PlaceWall(Position position)
        {
            this[position] = BoardElementType.Wall;
        }

        private void MovePlayer(QuoridorPlayer currentPlayer, Position position)
        {
            if (currentPlayer == null)
                return;

            this[currentPlayer.CurrentPosition] = BoardElementType.Empty;
            BoardMatrix[currentPlayer.CurrentPosition.Y][currentPlayer.CurrentPosition.X].Player = null;

            currentPlayer.CurrentPosition = position;

            this[currentPlayer.CurrentPosition] = BoardElementType.Player;
            BoardMatrix[currentPlayer.CurrentPosition.Y][currentPlayer.CurrentPosition.X].Player = currentPlayer;
        }

        private Position TranslatePlayerPosition(QuoridorPlayer player, Position pos)
        {
            if (player is AiPlayer || player.StartingPosition==Bottom)
                return pos;

            if(player.StartingPosition==Top)
                return new Position {X = BoardSize - pos.X, Y = BoardSize - pos.Y};
            else if(player.StartingPosition==Left)
                return new Position { X = BoardSize - pos.Y, Y = BoardSize - pos.X };//check
            else if (player.StartingPosition == Right)
                return new Position { X = pos.Y, Y = pos.X };//check
            else throw new Exception();
        }

        public static Position Bottom = new Position(BoardSize/2, BoardSize);
        public static Position Top = new Position(BoardSize / 2, 0);
        public static Position Left = new Position(0, BoardSize / 2);
        public static Position Right = new Position(BoardSize, BoardSize / 2);
    }
}