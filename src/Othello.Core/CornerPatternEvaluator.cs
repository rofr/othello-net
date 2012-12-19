namespace Othello.Core
{
    public class CornerPatternEvaluator : Evaluator
    {
        public override float Evaluate(Disc playerToMove, Board board)
        {

            int score = EvaluateCornerPattern(board["a1"], board["b1"], board["a2"], board["b2"])
                        + EvaluateCornerPattern(board["h8"], board["h7"], board["g8"], board["g7"])
                        + EvaluateCornerPattern(board["a8"], board["a7"], board["b8"], board["b7"])
                        + EvaluateCornerPattern(board["h1"], board["g1"], board["h2"], board["g2"]);

            if (playerToMove == Disc.White) score = -score;
            return score;

            //int squaresLeft = game.SquaresRemaining();


            ////TODO: make som predefined weight-arrays with better resolution
            //if (squaresLeft < 4) { return (material * 97 + mobility * 2 + position * 1) / 100; }
            //else if (squaresLeft < 10) { return (material * 10 + mobility * 10 + position * 80) / 100; }
            //else if (squaresLeft < 18) { return (material * 5 + mobility * 15 + position * 80) / 100; }
            //else if (squaresLeft < 26) { return (material * 2 + mobility * 20 + position * 78) / 100; }
            //else if (squaresLeft < 34) { return (material * 2 + mobility * 19 + position * 79) / 100; }
            //else if (squaresLeft < 42) { return (material * 0 + mobility * 15 + position * 85) / 100; }
            //else if (squaresLeft < 50) { return (material * 0 + mobility * 10 + position * 90) / 100; }
            //else { return (material * 0 + mobility * 10 + position * 90) / 100; }            
        }


        /// <summary>
        /// Calculate score from Blacks perspective
        /// </summary>
        private static int EvaluateCornerPattern(Disc corner, Disc edge1, Disc edge2, Disc diagonal)
        {

            //Todo: reimplement using lookup tables
            int value = 0;

            //if the corner is empty - big penalty for X-square, small penalty for C-square
            if (corner == Disc.None)
            {
                if (diagonal == Disc.Black) value -= 20;
                else if (diagonal == Disc.White) value += 20;
                return value;
            }
            //if the corner is occupied - bonus for each adjacent
            value = 20;
            if (corner == edge1) value += 10;
            if (corner == edge2) value += 10;
            if (corner == diagonal) value += 10;
            if (corner == Disc.White) value = -value;
            return value;
        }
    }
}