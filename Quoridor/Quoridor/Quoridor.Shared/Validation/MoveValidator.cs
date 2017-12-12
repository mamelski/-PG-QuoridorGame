using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Metadata;
using Quoridor.DataContracts;
using Quoridor.Model;

namespace Quoridor
{
    public static class MoveValidator
    {
        public static bool IsValid(BoardArray board, Position playerPosition, IEnumerable<Position> playersPositions, Position newPosition)
        {
            //cant move out of board
            if (!BasicValidator.IsWithinBoard(newPosition)) //just in case, it doesnt cost much
                return false;

            //cant only move to empty position
            if (board[newPosition] != BoardElementType.Empty)
                return false;


            //cant move to the same position or to the position of any of the oponents
            if (playersPositions.Any(p => p.Equals(newPosition)))
                return false;

            //cant move diagonal
            if (playerPosition.X != newPosition.X && playerPosition.Y != newPosition.Y)
                return false;


            foreach (var oponentPosition in playersPositions)
            {
                if (oponentPosition == playerPosition) continue;
                //can only move two fields if jumping over another pawn and there is no wall between
                if (oponentPosition.IsAdjacent(playerPosition) && oponentPosition.IsAdjacent(newPosition))
                {
                    if (playerPosition.Y == newPosition.Y &&
                        ((playerPosition.X - newPosition.X == 4 && playerPosition.X - oponentPosition.X == 2 && board[playerPosition.Y, playerPosition.X - 1] != BoardElementType.Wall && board[playerPosition.Y, playerPosition.X - 3] != BoardElementType.Wall) ||
                         (playerPosition.X - newPosition.X == -4 && playerPosition.X - oponentPosition.X == -2 && board[playerPosition.Y, playerPosition.X + 1] != BoardElementType.Wall && board[playerPosition.Y, playerPosition.X + 3] != BoardElementType.Wall)))
                        return true;


                    if (playerPosition.X == newPosition.X &&
                        ((playerPosition.Y - newPosition.Y == 4 && playerPosition.Y - oponentPosition.Y == 2 && board[playerPosition.Y - 1, playerPosition.X] != BoardElementType.Wall && board[playerPosition.Y - 3, playerPosition.X] != BoardElementType.Wall) ||
                         (playerPosition.Y - newPosition.Y == -4 && playerPosition.Y - oponentPosition.Y == -2 && board[playerPosition.Y + 1, playerPosition.X] != BoardElementType.Wall && board[playerPosition.Y + 3, playerPosition.X] != BoardElementType.Wall)))
                        return true;
                }
            }

            //cant move more than one field in x or y direction if not jumping over another pawn
            if (playerPosition.X == newPosition.X &&
                Math.Abs(playerPosition.Y - newPosition.Y) != 2)
                return false;
            if (playerPosition.Y == newPosition.Y &&
                Math.Abs(playerPosition.X - newPosition.X) != 2)
                return false;

            //cant move if wall is placed between current and new position
            if (playerPosition.X == newPosition.X &&
                ((playerPosition.Y + 2 == newPosition.Y &&
                  board[playerPosition.Y + 1, playerPosition.X] == BoardElementType.Wall) ||
                 (playerPosition.Y - 2 == newPosition.Y &&
                  board[playerPosition.Y - 1, playerPosition.X] == BoardElementType.Wall)))
                return false;

            if (playerPosition.Y == newPosition.Y &&
                ((playerPosition.X + 2 == newPosition.X &&
                  board[playerPosition.Y, playerPosition.X + 1] == BoardElementType.Wall) ||
                 (playerPosition.X - 2 == newPosition.X &&
                  board[playerPosition.Y, playerPosition.X - 1] == BoardElementType.Wall)))
                return false;

            return true;
        }

        [Deprecated("", DeprecationType.Deprecate, 1)]
        public static bool IsValid(Board board, Position playerPosition, Position oponentPosition, Position newPosition)
        {
            //cant move out of board
            if (!BasicValidator.IsWithinBoard(newPosition)) //just in case, it doesnt cost much
                return false;

            //cant only move to empty position
            if (board[newPosition] != BoardElementType.Empty)
                return false;


            //cant move to the same position
            if (playerPosition.X == newPosition.X && playerPosition.Y == newPosition.Y)
                return false;

            //cant move to the enemy position
            if (oponentPosition.X == newPosition.X && oponentPosition.Y == newPosition.Y)
                return false;

            //cant move diagonal
            if (playerPosition.X != newPosition.X && playerPosition.Y != newPosition.Y)
                return false;


            //can only move two fields if jumping over another pawn and there is no wall between
            if (oponentPosition.IsAdjacent(playerPosition) && oponentPosition.IsAdjacent(newPosition))
            {
                if (playerPosition.Y == newPosition.Y &&
                    ((playerPosition.X - newPosition.X == 4 &&
                      (playerPosition.X - oponentPosition.X != 2 ||
                       board.BoardMatrix[playerPosition.Y][playerPosition.X - 1].FieldType == BoardElementType.Wall ||
                       board.BoardMatrix[playerPosition.Y][playerPosition.X - 3].FieldType == BoardElementType.Wall)) ||
                     (playerPosition.X - newPosition.X == -4 &&
                      (playerPosition.X - oponentPosition.X != -2 ||
                       board.BoardMatrix[playerPosition.Y][playerPosition.X + 1].FieldType == BoardElementType.Wall ||
                       board.BoardMatrix[playerPosition.Y][playerPosition.X + 3].FieldType == BoardElementType.Wall))))
                    return false;


                if (playerPosition.X == newPosition.X &&
                    ((playerPosition.Y - newPosition.Y == 4 &&
                      (playerPosition.Y - oponentPosition.Y != 2 ||
                       board.BoardMatrix[playerPosition.Y - 1][playerPosition.X].FieldType == BoardElementType.Wall ||
                       board.BoardMatrix[playerPosition.Y - 3][playerPosition.X].FieldType == BoardElementType.Wall)) ||
                     (playerPosition.Y - newPosition.Y == -4 &&
                      (playerPosition.Y - oponentPosition.Y != -2 ||
                       board.BoardMatrix[playerPosition.Y + 1][playerPosition.X].FieldType == BoardElementType.Wall ||
                       board.BoardMatrix[playerPosition.Y + 3][playerPosition.X].FieldType == BoardElementType.Wall))))
                    return false;
            }

            else
            {
                //cant move more than one field in x or y direction if not jumping over another pawn
                if (playerPosition.X == newPosition.X &&
                    Math.Abs(playerPosition.Y - newPosition.Y) != 2)
                    return false;
                if (playerPosition.Y == newPosition.Y &&
                    Math.Abs(playerPosition.X - newPosition.X) != 2)
                    return false;

                //cant move if wall is placed between current and new position
                if (playerPosition.X == newPosition.X &&
                    ((playerPosition.Y + 2 == newPosition.Y &&
                      board.BoardMatrix[playerPosition.Y + 1][playerPosition.X].FieldType == BoardElementType.Wall) ||
                     (playerPosition.Y - 2 == newPosition.Y &&
                      board.BoardMatrix[playerPosition.Y - 1][playerPosition.X].FieldType == BoardElementType.Wall)))
                    return false;

                if (playerPosition.Y == newPosition.Y &&
                    ((playerPosition.X + 2 == newPosition.X &&
                      board.BoardMatrix[playerPosition.Y][playerPosition.X + 1].FieldType == BoardElementType.Wall) ||
                     (playerPosition.X - 2 == newPosition.X &&
                      board.BoardMatrix[playerPosition.Y][playerPosition.X - 1].FieldType == BoardElementType.Wall)))
                    return false;
            }


            return true;
        }
    }
}