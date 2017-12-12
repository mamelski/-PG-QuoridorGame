using System.Threading.Tasks;
using Quoridor.DataContracts;

namespace Quoridor.Model
{
    public class HumanPlayer : QuoridorPlayer
    {
        private readonly QuoridorWebService quoridorWebService;


        public HumanPlayer(Player user) : this(null, user)
        {
        }

        public HumanPlayer(PlayerParameters user) : this(null, user)
        {
        }
        public HumanPlayer(QuoridorWebService quoridorWebService, Player user) : base(user)
        {
            this.quoridorWebService = quoridorWebService;
        }

        public HumanPlayer(QuoridorWebService quoridorWebService, PlayerParameters user) : base(user)
        {
            this.quoridorWebService = quoridorWebService;
        }

        public override async Task<Move> GetMove()
        {
            if (quoridorWebService == null) return null;
            Move move = null;
            while (move == null)
            {
                move = await quoridorWebService.GetMove(Id);
            }
            return move;
        }

        public override async Task<bool> SendMove(Move move)
        {
            //move.Player = this;
            if (quoridorWebService == null) return false;
            move.Player = new Player() {Id=Id,Name=Name};
            return await this.quoridorWebService?.SendMove(move);
        }
    }
}