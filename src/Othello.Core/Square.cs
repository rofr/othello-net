using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

namespace Othello.Core
{
    public class Square
    {

        /// <summary>
        /// Zero based row
        /// </summary>
        public readonly int Row;

        /// <summary>
        /// Zero based column
        /// </summary>
        public readonly int Col;
        

        /// <summary>
        /// Each cell has 8 neighbors, one in each direction. Null if out of bounds
        /// </summary>
        private readonly Square[] _neighbors = new Square[8];

        /// <summary>
        /// There are 64 cells on a board
        /// </summary>
        private static Square[,] _allSquares = new Square[8,8];


        public IEnumerable<Square> Neighbors
        {
            get
            {
                foreach (var neighbor in _neighbors)
                {
                    yield return neighbor;
                }
            }
        }

        public static IEnumerable<Square> All
        {
            get
            {
                foreach (var square in _allSquares)
                {
                    yield return square;
                }
            }
        }

        private Square(int row, int col)
        {
            Row = row;
            Col = col;
        }


        static Square()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    _allSquares[row,col] = new Square(row,col);
                }
            }

            //Set references to neighbor cells
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Square cell = _allSquares[row,col];
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        cell._neighbors[(int)direction] = cell.CalculateSuccessor(direction);
                    }
                    
                }
            }

        }

        public static Square At(int row, int col)
        {
            return _allSquares[row, col];
        }


        public bool HasSuccessor(Direction direction)
        {
            return _neighbors[(int) direction] != null;
        }

        public Square Successor(Direction direction)
        {
            return _neighbors[(int) direction];
        }

        public static Square Parse(string square)
        {
            square = square.ToLower();
            if (!Regex.IsMatch(square, "^[a-h][1-8]$"))
                throw new ArgumentException("move");

            int col = "abcdefgh".IndexOf(square[0]);
            int row = "12345678".IndexOf(square[1]);
            return Square.At(row, col);
        }


        public override String ToString()
        {
            return String.Format("{0}{1}", "abcdefgh"[Col], Row + 1);
        }

        private Square CalculateSuccessor(Direction direction)
        {
            int row = Row;
            int col = Col;
            switch (direction)
            {
                case Direction.North:
                    row--;
                    break;
                case Direction.NorthEast:
                    row--;
                    col++;
                    break;
                case Direction.East:
                    col++;
                    break;
                case Direction.SouthEast:
                    row++;
                    col++;
                    break;
                case Direction.South:
                    row++;
                    break;
                case Direction.SouthWest:
                    row++;
                    col--;
                    break;
                case Direction.West:
                    col--;
                    break;
                case Direction.NorthWest:
                    row--;
                    col--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
            if (row < 0 || row >= 8 || col < 0 || col >= 8) return null;
            return Square.At(row, col);
        }
    }
}