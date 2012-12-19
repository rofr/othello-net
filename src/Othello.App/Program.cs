using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Othello.Core;

namespace Othello.App
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayGame();


            //Disc[,] discs = new Disc[8,8];
            //Board board = new Board(discs);

            //board["a1"] = Disc.Black;
            //board["b1"] = Disc.Black;
            //board["c1"] = Disc.Black;
            //board["d1"] = Disc.Black;
            //board["e1"] = Disc.Black;
            //board["f1"] = Disc.Black;

            //board["a2"] = Disc.Black;
            //board["b2"] = Disc.White;
            //board["c2"] = Disc.Black;
            //board["d2"] = Disc.Black;
            //board["e2"] = Disc.Black;
            //board["f2"] = Disc.Black;

            //board["a3"] = Disc.Black;
            //board["b3"] = Disc.White;
            //board["c3"] = Disc.White;
            //board["d3"] = Disc.White;
            //board["e3"] = Disc.White;
            //board["f3"] = Disc.White;
            //board["g3"] = Disc.White;

            //board["a4"] = Disc.Black;
            //board["b4"] = Disc.Black;
            //board["c4"] = Disc.Black;
            //board["d4"] = Disc.White;
            //board["e4"] = Disc.White;
            //board["f4"] = Disc.White;

            //board["b5"] = Disc.Black;
            //board["c5"] = Disc.White;
            //board["d5"] = Disc.White;
            //board["e5"] = Disc.White;

            //board["a6"] = Disc.Black;
            //board["b6"] = Disc.White;
            //board["c6"] = Disc.White;
            //board["d6"] = Disc.White;

            //board["a7"] = Disc.White;

            //Drawboard(board);
            //PrintMoves(board.Moves(Disc.Black));
            //Console.ReadLine();
            //board.MakeMove(Disc.Black, Square.Parse("h4"));
            //Drawboard(board);
            //Console.ReadLine();


        }

        private static void PlayGame()
        {
            Game game = new Game();
            game.Pass += (s, e) =>
                             {
                                 Disc player = game.State.PlayerToMove().Opponent();
                                 Console.WriteLine("pass, " + player + " has no moves");
                             };
            Disc computer = Disc.Black;
            CompositeEvaluator nodeEvaluator = new CompositeEvaluator();
            nodeEvaluator.Evaluators.Add(new WeightedEvaluator(1, new MaterialEvaluator()));
            nodeEvaluator.Evaluators.Add(new WeightedEvaluator(10, new CornerPatternEvaluator()));
            nodeEvaluator.Evaluators.Add(new WeightedEvaluator(10, new MobilityEvaluator()));

            AlphabetaEvaluator computerPlayer = new AlphabetaEvaluator(7, nodeEvaluator);

            while (!game.Over)
            {
                Drawboard(game.Board);

                Disc playerToMove = game.State.PlayerToMove();
                if (playerToMove == computer)
                {
                    float score = computerPlayer.Evaluate(playerToMove, game.Board);
                    Console.WriteLine("Score: " + score);
                    Console.WriteLine("Nodes evaluated: " + computerPlayer.NodesEvaluated);
                    Console.WriteLine("Beta cutoffs: " + computerPlayer.BetaCutoffs);
                    Console.WriteLine("Move: " + computerPlayer.BestMove);
                    game.MakeMove(computerPlayer.BestMove);
                }
                else
                {
                    while (true)
                    {
                        Console.Write("Moves: ");
                        PrintMoves(game.Board.Moves(playerToMove));
                        Console.Write("Your move: ");
                        string input = Console.ReadLine().ToLower();
                        if (input == "quit")
                        {
                            game.Resign();
                            break;
                        }
                        try
                        {
                            Square move = Square.Parse(input);
                            GameState newState = game.MakeMove(move);
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid move, try again");
                        }
                    }
                }
            }
            Drawboard(game.Board);
            Console.WriteLine("Game over: " + game.State);
            Console.WriteLine("Black: " + game.Board.Count(Disc.Black) + " White: " + game.Board.Count(Disc.White));
            Console.ReadLine();
        }

        private static void PrintMoves(Square[] moves )
        {
            foreach (var move in moves)
            {
                Console.Write(move + " ");
            }
            Console.WriteLine();
        }

        private static void Drawboard(Board board)
        {
            Console.Write("  ");
            char[] ch = { '-', 'O', 'X' };
            for (int col = 0; col < 8; col++)
                Console.Write(String.Format("{0} ", (char)('a' + col)));

            Console.WriteLine();
            for (int row = 1; row <= 8; row++)
            {
                Console.Write(row);
                for (int col = 0; col < 8; col++)
                    Console.Write(" " + ch[(int)board[Square.At(row-1, col)]]);
                Console.WriteLine();
            }

        }
    }
}
