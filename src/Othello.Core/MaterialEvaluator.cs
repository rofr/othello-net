namespace Othello.Core
{
    public class MaterialEvaluator : Evaluator
    {
        public override float Evaluate(Disc playerToMove, Board board)
        {
            int playersDiscs = board.Count(playerToMove);
            int opponentDiscs = 64 - ( playersDiscs + board.Count(Disc.None));
            return playersDiscs - opponentDiscs;
        }
    }
}