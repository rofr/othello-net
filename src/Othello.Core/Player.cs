using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Othello.Core
{
    public abstract class Player
    {
        public abstract string GetMove(Board game);
    }

    public class ConsolePlayer : Player
    {

        public ConsolePlayer(Game controller)
        {
            
        }

        public override string GetMove(Board game)
        {
            return "a1";
        }
    }
}
