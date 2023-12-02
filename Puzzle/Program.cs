using System;
using System.Runtime.CompilerServices;
namespace Puzzle
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //initialize puzzle
            //testing
            int[,] quizTiles = { 
                { 1, 2, 3 }, 
                { 8, 0, 4 }, 
                { 7, 6, 5 } 
            };
            //initialize
            //int[,] quizTiles = new int[3, 3];
            Puzzle puzzle = new Puzzle(quizTiles);
            //fill the tile with random number 0 - 8
            //puzzle.FillPuzzle();
            //print initial State of puzzle
            Console.WriteLine("Initial State:");
            puzzle.PrintBoard();
            foreach(string move in puzzle.AvailableMoves())
            {
                Console.WriteLine(move);
            }
            puzzle.SwapTiles(puzzle.AvailableMoves()[0]);
            puzzle.PrintBoard();
        }
    }
}
