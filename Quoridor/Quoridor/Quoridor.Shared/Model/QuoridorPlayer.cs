using System;
using System.Threading.Tasks;
using Windows.UI;
using Quoridor.DataContracts;
using Quoridor.Utils;

namespace Quoridor.Model
{
    public abstract class QuoridorPlayer
    {
        protected QuoridorPlayer(Player user) : this(user.Id, user.Name)
        {
        }

        protected QuoridorPlayer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        protected QuoridorPlayer(PlayerParameters playerParameters)
        {
            StartingPosition = CurrentPosition = GetPosition(playerParameters.StartingPosition);
            PawnColor = playerParameters.PawnColor;
            Name = playerParameters.Name;
            Id = new Guid(); // RandomProvider.GetRandomNumber();
           
        }

        private Position GetPosition(PlayerStartingPosition startingPosition)
        {
            switch (startingPosition)
            {
                case PlayerStartingPosition.Bottom:
                    return Board.Bottom;
                case PlayerStartingPosition.Top:
                    return Board.Top;
                case PlayerStartingPosition.Right:
                    return Board.Right;
                case PlayerStartingPosition.Left:
                    return Board.Left;
                default:
                    throw new ArgumentOutOfRangeException(nameof(startingPosition), startingPosition, null);
            }
        }

        public Position StartingPosition { get; set; }

        public Position CurrentPosition { get; set; }

        public Color PawnColor { get; set; }

        public int NumberOfWallsAvalaible { get; set; } = 10;

        public bool HasWon => IsWiningPosition(CurrentPosition);

        public string Name { get; set; }

        public Guid Id { get; set; }

        public bool IsWiningPosition(Position position)
        {
            return Math.Abs(position.Y - StartingPosition.Y) == Board.BoardSize || Math.Abs(position.X - StartingPosition.X) == Board.BoardSize;
        }

        public virtual async Task<bool> SendMove(Move move)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        public virtual async Task<Move> GetMove()
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}