using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuoridorClient.Enums;

namespace QuoridorClient
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Board
    {
        enum PlayerType
        {
            User,
            Opponent
        }

        public const int BOARD_SIZE = 9;

        //private int[,] _board = new int[BOARD_SIZE, BOARD_SIZE];

        public Position UserPosition { get; private set; }

        public Position OpponentPosition { get; private set; }

        /// <summary>
        /// Board size in pixels.
        /// </summary>
        public int BoardSizePx { get; private set; }

        public int FieldSizePx { get; private set; }

        private int _marginPx;
        private Vector2 _startingPoint;

        public Board(int boardSizePx)
        {
            BoardSizePx = boardSizePx;
            UserPosition = new Position(4, 8);
            OpponentPosition = new Position(4, 0);

            _marginPx = 5;
            FieldSizePx = (BoardSizePx - 8*_marginPx)/9;

            var width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            //_startingPoint = new Vector2(width/2 - BoardSizePx/2, height/2 - BoardSizePx/2);
            _startingPoint = new Vector2(100, 100);
        }

        public void MoveUser(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Forward:
                    UserPosition.Y--;
                    break;
                case MoveDirection.Backward:
                    UserPosition.Y++;
                    break;
                case MoveDirection.Right:
                    UserPosition.X++;
                    break;
                case MoveDirection.Left:
                    UserPosition.X--;
                    break;
            }
        }

        public void MoveOpponent(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Forward:
                    OpponentPosition.Y++;
                    break;
                case MoveDirection.Backward:
                    OpponentPosition.Y--;
                    break;
                case MoveDirection.Right:
                    OpponentPosition.X--;
                    break;
                case MoveDirection.Left:
                    OpponentPosition.X++;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">X index</param>
        /// <param name="y">Y index</param>
        /// <returns></returns>
        public Vector2 GetFieldPosition(int x, int y)
        {
            int i_x = x*FieldSizePx + x*_marginPx;
            int i_y = y*FieldSizePx + y*_marginPx;

            return new Vector2(i_x + _startingPoint.X, i_y + _startingPoint.Y);
        }

        public Vector2 GetUserPositionPx()
        {
            return PawnCoordsPx(UserPosition.X, UserPosition.Y);
        }

        public Vector2 GetOpponentPositionPx()
        {
            return PawnCoordsPx(OpponentPosition.X, OpponentPosition.Y);
        }

        private Vector2 PawnCoordsPx(int x, int y)
        {
            var coord = GetFieldPosition(x, y);
            coord.X += FieldSizePx / 2 - 10;
            coord.Y += FieldSizePx / 2 - 10;
            return coord;
        }
    }
}
