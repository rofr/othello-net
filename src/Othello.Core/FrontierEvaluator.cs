using System.Linq;

namespace Othello.Core
{

    public class FrontierEvaluator : Evaluator
    {
        public override float Evaluate(Disc playerToMove, Board board)
        {
            int frontierDiscComparison = Square.All
                .Where(square => board[(Square)square] != Disc.None)
                .Sum(square => square.Neighbors
                        .Count(neighbor => neighbor != null && board[neighbor] == Disc.None) *
                         (board[square] == playerToMove ? -1 : 1));
            return frontierDiscComparison;
        }
    }
}