using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Othello.Core
{
    public class WeightedEvaluator : Evaluator
    {
        public readonly int Weight;
        public readonly Evaluator Evaluator;

        public WeightedEvaluator(int weight, Evaluator evaluator)
        {
            Weight = weight;
            Evaluator = evaluator;
        }

        public override float Evaluate(Disc playerToMove, Board board)
        {
            return Evaluator.Evaluate(playerToMove, board) * Weight;
        }
    }
}
