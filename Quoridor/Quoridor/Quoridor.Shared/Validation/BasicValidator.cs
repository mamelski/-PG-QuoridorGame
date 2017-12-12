using Quoridor.DataContracts;
using Quoridor.Model;

namespace Quoridor
{
    public static class BasicValidator
    {
        public static bool IsWithinBoard(Position pos)
        {
            return pos.X >= 0 && pos.Y >= 0 && pos.X <= Board.BoardSize && pos.Y <= Board.BoardSize;
        }
    }
}