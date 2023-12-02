using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle
{
    public class Puzzle
    {
        private readonly int[,] goalBoard1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
        private readonly int[,] goalBoard2 = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
        public int[,] board { get; set; }
        public int emptyRow { get; set; }
        public int emptyCol { get; set; }

        public Puzzle(int[,] board)
        {
            this.board = board;
        }

        public void findEmpty()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == 0)
                    {
                        emptyRow = row;
                        emptyCol = col;
                    }
                }
            }
        }

        public void PrintBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write($"{board[row, col]}    ");
                }
                Console.WriteLine();
            }
        }

        public  void FillPuzzle()
        {
            int[,] tiles = new int[3, 3];
            Random rand = new Random();
            //List with all the number 0 to 8
            List<int> numbers = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            //shuffle the list
            numbers = numbers.OrderBy(x => rand.Next()).ToList();
            //fill the Quiz with shuffled numbers
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = numbers[i * 3 + j];
                }
            }
        }

        public List<string> AvailableMoves()
        {
            List<string> AMoves = new();
            //available moves
            // u = up
            // d = down
            // l = left
            // r = right
            findEmpty();
            if (emptyRow > 0 && emptyRow < 2)
            {
                AMoves.Add("u");
                AMoves.Add("d");
            }
            if (emptyCol > 0 && emptyCol < 2)
            {
                AMoves.Add("l");
                AMoves.Add("r");
            }
            if (emptyCol == 0)
            {
                AMoves.Add("r");
            }
            if (emptyRow == 0)
            {
                AMoves.Add("d");
            }
            if (emptyCol == 2)
            {
                AMoves.Add("l");
            }
            if (emptyRow == 2)
            {
                AMoves.Add("u");
            }
            return AMoves;
        }

        public void SwapTiles(string move)

        {
            findEmpty();
            switch (move)
            {
                case "u":
                    board[emptyRow, emptyCol] = board[emptyRow - 1, emptyCol];
                    board[--emptyRow, emptyCol] = 0;
                    break;
                case "d":
                    board[emptyRow, emptyCol] = board[emptyRow + 1, emptyCol];
                    board[++emptyRow, emptyCol] = 0;
                    break;
                case "l":
                    board[emptyRow, emptyCol] = board[emptyRow, emptyCol - 1];
                    board[emptyRow, --emptyCol] = 0;
                    break;
                case "r":
                    board[emptyRow, emptyCol] = board[emptyRow, emptyCol + 1];
                    board[emptyRow, ++emptyCol] = 0;
                    break;
            }
        }

        public bool IsSolved()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] != goalBoard1[row, col] && board[row, col] != goalBoard2[row, col])
                    {
                        return false;
                    }
                } 
            }
            return true;
        }
    }
}
