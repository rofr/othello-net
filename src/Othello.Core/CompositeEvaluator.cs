using System.Collections.Generic;

namespace Othello.Core
{
    public class CompositeEvaluator : Evaluator
    {
        public readonly List<WeightedEvaluator> Evaluators = new List<WeightedEvaluator>();

        public override float Evaluate(Disc playerToMove, Board board)
        {
            int divisor = 0;
            float total = 0;
            foreach (var evaluator in Evaluators)
            {
                total += evaluator.Evaluate(playerToMove, board);
                divisor += evaluator.Weight;
            }
            if (divisor == 0) return 0;
            return total / divisor;
        }
    }
}