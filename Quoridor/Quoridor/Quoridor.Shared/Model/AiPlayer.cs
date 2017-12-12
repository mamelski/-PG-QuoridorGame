using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Quoridor.DataContracts;
using Quoridor.Utils;

namespace Quoridor.Model
{
    public class AiPlayer : QuoridorPlayer
    {
        private Random random = new Random();
        private Board board;

        private double _probabilityOfWallPlacement = 0.05;
        private Position movePosition = null;
        private Position[] wallPositions = new Position[3];



        public AiPlayer(Board board,PlayerParameters playerParameters) : base(playerParameters)
        {
            this.board = board;
        }

        // TODO zamiast inta jest GUID trzeba to rozwiązać
        public AiPlayer(Board board) : base(Guid.NewGuid(), "AI_Player_" + RandomProvider.LastNumber)
        {
            this.board = board;
        }


        private void RollNewPosition()
        {
            movePosition = new Position()
            {
                X = 2 * (random.Next(0, Board.BoardSize + 1) / 2),
                Y = 2 * (random.Next(0, Board.BoardSize + 1) / 2)
            };

            wallPositions[1] = new Position(2 * (random.Next(0, Board.BoardSize+1)/2) + 1, 2 * (random.Next(0, Board.BoardSize + 1) / 2) + 1);//this should be microwall

            if (random.NextDouble() < 0.5)
            {
                wallPositions[0] = new Position(wallPositions[1].X, wallPositions[1].Y-1);
                wallPositions[2] = new Position(wallPositions[1].X, wallPositions[1].Y + 1);
            }
            else
            {
                wallPositions[0] = new Position(wallPositions[1].X-1, wallPositions[1].Y);
                wallPositions[2] = new Position(wallPositions[1].X+1, wallPositions[1].Y);
            }

        }


        public override async Task<Move> GetMove()
        {
            Move newMove = null;
           
            await Task.Run(() =>
            {
                var boardCopy = board.GetBoardArray();

                do
                {
                    RollNewPosition();

                    if (random.NextDouble() <= _probabilityOfWallPlacement && NumberOfWallsAvalaible>0 && WallValidator.AreValid(boardCopy, board.Players, this, wallPositions))
                    {
                        newMove = new Move() {IsWallPlacement = true,WallPlacementPositions = wallPositions};
                    }
                    else if(MoveValidator.IsValid(boardCopy, CurrentPosition, board.PlayersPositions, movePosition))
                    {
                        newMove = new Move() { Destination = movePosition };
                    }

                } while (newMove==null);

                System.Threading.Tasks.Task.Delay(500).Wait();//AI is thinking :D

                if (newMove.IsWallPlacement)
                    NumberOfWallsAvalaible--;
            });
            
            return newMove;
        }

        
    }
}
