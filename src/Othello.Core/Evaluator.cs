using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Othello.Core
{
    public abstract class Evaluator
    {
        public abstract float Evaluate(Disc playerToMove, Board board);
    }
}
