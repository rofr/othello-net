using System;
using System.Collections.Generic;
using System.Text;

namespace Othello.Core
{
    public class AlphabetaEvaluator : Evaluator
    {

        public int NodesEvaluated
        {
            get;
            private set;
        }

        public long BetaCutoffs
        {
            get;
            private set;
        }

        public bool IterativeDeepening { get; set; }


        public readonly int MaxSearchDepth;
        private int _currentSearchDepth;

        private readonly Evaluator _nodeEvaluator;

        public AlphabetaEvaluator(int maxSearchDepth, Evaluator nodeEvaluator)
        {
            MaxSearchDepth = maxSearchDepth;
            _nodeEvaluator = nodeEvaluator;
        }



        public float BestScore
        {
            get;
            private set;
        }

        public Square BestMove
        {
            get;
            private set;
        }


        private float Alphabeta(Board board, Disc playerToMove, int distanceToLeafNodes, float alpha, float beta)
        {
            if (distanceToLeafNodes == 0)
            {
                NodesEvaluated++;
                return _nodeEvaluator.Evaluate(playerToMove, board);
            }

            float best = float.MinValue;
            foreach (Square move in board.Moves(playerToMove))
            {
                if (best >= beta)
                {
                    BetaCutoffs++;
                    break;
                }
                var childBoard = new Board(board);
                childBoard.MakeMove(playerToMove, move);

                if (best > alpha) alpha = best;
                float score = -Alphabeta(childBoard, playerToMove.Opponent(), distanceToLeafNodes - 1, -beta, -alpha);
                if (score > best)
                {
                    best = score;
                    if (distanceToLeafNodes == _currentSearchDepth)
                    {
                        BestScore = score;
                        BestMove = move;
                    }

                }
            }
            return best;
        }


        public override float Evaluate(Disc playerToMove, Board board)
        {

            NodesEvaluated = 0;
            BetaCutoffs = 0;

            if (IterativeDeepening) _currentSearchDepth = 1;
            else _currentSearchDepth = MaxSearchDepth;

            while (_currentSearchDepth <= MaxSearchDepth)
            {
                _currentSearchDepth++;
                Alphabeta(board, playerToMove, _currentSearchDepth, float.MinValue, float.MaxValue);
            }

            return BestScore;

        }
    }

}
