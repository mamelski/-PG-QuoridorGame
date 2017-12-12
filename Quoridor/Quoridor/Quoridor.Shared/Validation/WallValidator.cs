using System.Collections.Generic;
using System.Linq;
using Quoridor.DataContracts;
using Quoridor.Model;

namespace Quoridor
{
    public static class WallValidator
    {
        private static readonly Position[] steps =
        {
            new Position(-2, 0), new Position(0, -2), new Position(2, 0), new Position(0, 2),
            new Position(-4, 0), new Position(0, -4), new Position(4, 0), new Position(0, 4)
        };


        public static bool IsPassable(BoardArray boardArray, IEnumerable<QuoridorPlayer> players)
        {
            // return true;
            //var isPassableForPlayer = false;
            //var isPassableForOpponent = false;


            foreach (var player in players)
            {
                var isPassableForPlayer = false;
                for (var x = 0; x <= Board.BoardSize; x += 2)
                {
                    for (var y = 0; y <= Board.BoardSize; y += 2)
                    {
                        var pos = new Position(x, y);
                        if (player.IsWiningPosition(pos) &&
                            IsPassable(boardArray, player.CurrentPosition, players.Select(p => p.CurrentPosition), pos))
                        {
                            isPassableForPlayer = true;
                            break;
                        }
                    }
                    if (isPassableForPlayer)
                        break;
                }
                if (!isPassableForPlayer)
                    return false;
            }

            return true;
        }


        public static bool IsPassable(BoardArray boardArray, Position fromPosition,
            IEnumerable<Position> oponentPositions, Position toPosition)
        {
            if (!BasicValidator.IsWithinBoard(fromPosition) || !BasicValidator.IsWithinBoard(toPosition))
                return false;
            var discovered = new HashSet<Position>();

            var s = new Stack<Position>();
            s.Push(fromPosition);

            while (s.Count > 0)
            {
                var pos = s.Pop();
                if (discovered.Contains(pos)) continue;

                discovered.Add(pos);

                foreach (var step in steps)
                {
                    var newPosition = pos + step;
                    if (MoveValidator.IsValid(boardArray, pos, oponentPositions, newPosition))
                        s.Push(newPosition);
                }
            }

            return discovered.Contains(toPosition);
        }


        public static bool AreValid(BoardArray boardArray, IEnumerable<QuoridorPlayer> players,
            QuoridorPlayer player, params Position[] wallPositions)
        {
            if (player.NumberOfWallsAvalaible <= 0)
                return false;

            //var boardCopy = new Board(board);
            foreach (var wallPosition in wallPositions)
            {
                if (!BasicValidator.IsWithinBoard(wallPosition)) //just in case, it doesnt cost much
                    return false;

                if (boardArray[wallPosition] != BoardElementType.EmptyForWall)
                    return false;
            }

            var elements = new BoardElementType[wallPositions.Length];

            for (var index = 0; index < wallPositions.Length; index++)
            {
                elements[index] = boardArray[wallPositions[index]];
                boardArray[wallPositions[index]] = BoardElementType.Wall;
            }

            var isPassable = IsPassable(boardArray, players);

            for (var index = 0; index < wallPositions.Length; index++)
                boardArray[wallPositions[index]] = elements[index];

            return isPassable;
        }
    }
}