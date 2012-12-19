using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Othello.Core
{

    /// <summary>
    /// The othello playing board
    /// </summary>
    public class Board
    {
        private readonly Disc[,] _squares = new Disc[8, 8];

        /// <summary>
        /// The number of unoccupied squares remaining
        /// </summary>
        int _squaresRemaining = 64 - 4;

        //Calculate when requested only
        private int _whiteDiscs = -1;
        private int _blackDiscs = -1;


        private bool IsMove(Disc toMove, Square move)
        {
            if (_squares[move.Row, move.Col] != Disc.None) return false;
            Disc opponent = toMove.Opponent();

            
            for (Direction dir = 0; dir <= Direction.NorthWest; dir++)
            {
                Square s2 = move.Successor(dir);
                if (s2 != null && _squares[s2.Row, s2.Col] == opponent)
                    do
                    {
                        s2 = s2.Successor(dir);
                        if (s2 != null && _squares[s2.Row, s2.Col] == toMove) return true;
                    } while (s2 != null && _squares[s2.Row, s2.Col] == opponent);
            }
            return false;
        }

        public Square[] Moves(Disc playerToMove)
        {
            List<Square> moves = new List<Square>();
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Square square = Square.At(row, col);
                    if (IsMove(playerToMove, square))
                    {
                        moves.Add(square);
                    }
                }
            }
            return moves.ToArray();
        }


        public Board(Board board)
        {
            //Copy members
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    _squares[row, col] = board._squares[row, col];
            _squaresRemaining = CountDiscs(Disc.None);
        }

        public Board()
        {
            _squares[3, 3] = Disc.White;
            _squares[4, 4] = Disc.White;
            _squares[3, 4] = Disc.Black;
            _squares[4, 3] = Disc.Black;
        }

        public Board(Disc[,] discs)
        {
            _squares = discs;
            _squaresRemaining = CountDiscs(Disc.None);
        }

        private void FlipBetween(Square start, Square end, Direction direction)
        {
            while (true)
            {
                start = start.Successor(direction);
                if (start.Row == end.Row && start.Col == end.Col) break;
                _squares[start.Row, start.Col] = _squares[start.Row, start.Col].Opponent();
            }
        }

        public int SquaresRemaining
        {

            get { return _squaresRemaining; }
        }

        public void MakeMove(Disc playerToMove, Square move)
        {
            if (playerToMove == Disc.None) throw new InvalidEnumArgumentException("playerToMove");
            _squaresRemaining--;

            //clear cached counts
            _blackDiscs = -1;
            _whiteDiscs = -1;

            //place the disc
            _squares[move.Row, move.Col] = playerToMove;

            Disc opponent = playerToMove.Opponent();

            //scan each direction
            for (Direction dir = 0; dir <= Direction.NorthWest; dir++)
            {
                Square cursor = move.Successor(dir);
                if (cursor != null && _squares[cursor.Row, cursor.Col] == opponent)
                    do
                    {
                        cursor = cursor.Successor(dir);
                        if (cursor != null && _squares[cursor.Row, cursor.Col] == playerToMove)
                        {
                            FlipBetween(move, cursor, dir); 
                            break;
                        }
                    } while (cursor != null && _squares[cursor.Row, cursor.Col] == opponent);
            }

        }

        public bool IsGameOver
        {
            get 
            {
                return _squaresRemaining == 0 
                    || Moves(Disc.White).Length == 0 && Moves(Disc.Black).Length == 0;
            }
        }

        public int Count(Disc disc)
        {
            if (disc == Disc.None) return _squaresRemaining;
            if (disc == Disc.White) return WhiteDiscs;
            if (disc == Disc.Black) return BlackDiscs;
            throw new InvalidEnumArgumentException("disc");
        }

        private int WhiteDiscs
        {
            get
            {
                if (_whiteDiscs == -1) _whiteDiscs = CountDiscs(Disc.White);
                return _whiteDiscs;
            }
        }

        private int BlackDiscs
        {
            get
            {
                if (_blackDiscs == -1) _blackDiscs = CountDiscs(Disc.Black);
                return _blackDiscs;
            }
        }

        private int CountDiscs(Disc disc)
        {
            int count = 0;
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    if (_squares[row, col] == disc) count++;
            return count;

        }


        private Disc this[int row, int col]
        {
            get
            {
                return _squares[row, col];
            }
        }

        public Disc this[Square square]
        {
            get
            {
                return this[square.Row, square.Col];
            }
            set
            {
                _squares[square.Row, square.Col] = value;
                _squaresRemaining = CountDiscs(Disc.None);
            }
        }

        public Disc this[string square]
        {
            get
            {
                return this[Square.Parse(square)];
            }
            set { this[Square.Parse(square)] = value; }
        }
    }
}