using System;
using System.Runtime.CompilerServices;
namespace Puzzle
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //initialize puzzle
            Console.WriteLine("Ο Υπολογισμός για την Επίληση του <<Το πρόβλημα των 8 γρίφων>> ξεκίνησε......");
            Console.WriteLine("Υπολογισμός Βάρους κάθε Κίνηση με Βάση την Απόσταση απο την πραγματική Θέση που΄πρέπει να είναι ο Αριθμός");
            //fill the tile with random number 0 - 8
            int[,] initialState = FillPuzzle();
            Puzzle puzzle = new Puzzle(initialState);
            puzzle.IDAStarSearch();
        }

        public static int[,] FillPuzzle()
        {
            int[,] initialState = new int[3, 3];
            Random rand = new Random();
            // Fill the puzzle with numbers 0 to 8
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    initialState[i, j] = i * 3 + j;
                }
            }
            // Shuffle the puzzle by swapping elements
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int randRow = rand.Next(3);
                    int randCol = rand.Next(3);
                    // Swap elements
                    int temp = initialState[i, j];
                    initialState[i, j] = initialState[randRow, randCol];
                    initialState[randRow, randCol] = temp;
                }
            }
            return initialState;
        }
    }
}
