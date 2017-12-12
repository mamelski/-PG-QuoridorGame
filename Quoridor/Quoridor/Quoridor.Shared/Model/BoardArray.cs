using Quoridor.DataContracts;

namespace Quoridor.Model
{
    public class BoardArray
    {
        public BoardElementType this[Position position]
        {
            get { return _array[position.Y,position.X]; }
            set { _array[position.Y,position.X] = value; }
        }

        public BoardElementType this[int y, int x]
        {
            get { return _array[y, x]; }
            set { _array[y, x] = value; }
        }

        private BoardElementType[,] _array = new BoardElementType[Board.BoardSize+1, Board.BoardSize+1];
    }
}