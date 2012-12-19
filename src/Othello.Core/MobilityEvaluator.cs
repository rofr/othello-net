using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Othello.Core
{
    public class MobilityEvaluator : Evaluator
    {
        public override float Evaluate(Disc playerToMove, Board board)
        {
            int movesAvailable = board.Moves(playerToMove).Length;
            return (float) Math.Min(Math.Pow(movesAvailable - 4, 3), 20);
        }
    }
}
