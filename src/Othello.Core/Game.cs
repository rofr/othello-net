using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Othello.Core
{
    public static class Ext
    {
        public static Disc Opponent(this Disc disc)
        {
            if (disc == Disc.White) return Disc.Black;
            if (disc == Disc.Black) return Disc.White;
            throw new InvalidEnumArgumentException("disc");
        }

        public static Disc PlayerToMove(this GameState state)
        {
            if (state == GameState.WhitesTurn) return Disc.White;
            if (state == GameState.BlacksTurn) return Disc.Black;
            throw new InvalidEnumArgumentException("state");
        }


        public static GameState TogglePlayer(this GameState state)
        {
            if(state == GameState.WhitesTurn) return GameState.BlacksTurn;
            if(state == GameState.BlacksTurn) return GameState.WhitesTurn;
            throw new InvalidEnumArgumentException("state");
        }

        public static GameState Win(this GameState state)
        {
            if (state == GameState.WhitesTurn) return GameState.WhiteWon;
            if (state == GameState.BlacksTurn) return GameState.BlackWon;
            throw new InvalidEnumArgumentException("state");
        }
        
        public static GameState Resign(this GameState state)
        {
            if (state == GameState.WhitesTurn) return GameState.BlackWon;
            if (state == GameState.BlacksTurn) return GameState.WhiteWon;
            throw new InvalidEnumArgumentException("state");
        }
    }

    public class Game
    {

        public event EventHandler<EventArgs> Pass = delegate { }; 

        public GameState State { get; private set; }

        private readonly List<string> _moveHistory = new List<string>();
        public readonly Board Board = new Board();

        public GameState MakeMove(Square move)
        {
            
            Disc playerToMove = State.PlayerToMove();
            Square[] validMoves = Board.Moves(playerToMove);
            
            if (!validMoves.Any(m => move.Col == m.Col && m.Row == move.Row))
            {
                throw new InvalidOperationException("Invalid move");
            }
            

            _moveHistory.Add(move.ToString());
            Board.MakeMove(playerToMove, move);


            if (Board.IsGameOver)
            {
                var score = (int)new MaterialEvaluator().Evaluate(playerToMove, Board);
                if (score > 0) State = State.Win();
                else if (score < 0) State = State.TogglePlayer().Win();
                else State = GameState.Draw;
            }
            else if (Board.Moves(playerToMove.Opponent()).Length == 0)
            {
                Pass.Invoke(this, EventArgs.Empty);
                _moveHistory.Add("pass");

            }
            else State = State.TogglePlayer();
            return State;
        }

        public Game()
        {
            State = GameState.BlacksTurn;
        }

        public bool Over
        {
            get 
            { 
                return State > GameState.WhitesTurn;
            }
        }


        public void Resign()
        {
            if (Over) throw new InvalidOperationException("Cant resign, game already over");
            State = State.Resign();
        }
    }
}