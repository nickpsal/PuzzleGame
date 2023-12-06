using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Puzzle
{
    [TestClass]
    // Puzzle class contains the logic for solving the 8-puzzle using IDA* search
    public class Puzzle
    {
        private readonly int[,] goalState1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
        private readonly int[,] goalState2 = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
        public int[,] initialState { get; set; }
        public int emptyRow { get; set; }
        public int emptyCol { get; set; }
        public int[] directions = { -3, -1, 1, 3 }; // Possible moves: Up, Left, Right, Down
        private Dictionary<string, int> memoizedHeuristics = new Dictionary<string, int>();

        // Constructor initializes the initial state of the puzzle
        public Puzzle()
        {
            Console.WriteLine("Ο Υπολογισμός για την Επίληση του <<Το πρόβλημα των 8 γρίφων>> ξεκίνησε");
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine("Υπολογισμός Βάρους κάθε Κίνηση με Βάση την Απόσταση απο την πραγματική Θέση που πρέπει να είναι ο Αριθμός");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            //fill the tile with random number 0 - 8
            this.initialState = InitializePuzzle();
            IDAStarSearch();
        }

        public int[,] InitializePuzzle()
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

        // IDA* search algorithm
        [TestMethod]
        public void IDAStarSearch()
        {
            int result = 0;
            int cost = CalculateHeuristic(initialState);
            Node root = new Node(initialState, cost, 0, null);
            int threshold = cost;
            while (true)
            {
                Console.WriteLine($"Τρέχον Βάρος Βαση Απόστασης απο την Τελική Θέση: {threshold}");
                result = DepthLimitedSearch(root, threshold);
                if (result == int.MaxValue)
                {
                    Console.WriteLine("Solution not found!");
                    return;
                }
                if (result < 0)
                {
                    Console.WriteLine("Solution found!");
                    return;
                }
                threshold = result;
            }
        }

        // Converts the state array to a string for memoization
        private string StateToString(int[,] state)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    sb.Append(state[i, j]);
                    sb.Append(','); // Add a delimiter to distinguish between elements
                }
            }
            return sb.ToString();
        }

        // Depth-limited search within a specified threshold
        private int DepthLimitedSearch(Node node, int threshold)
        {
            int f = node.Depth + CalculateHeuristic(node.State);
            if (f > threshold)
                return f;
            if (IsGoalState(node.State))
            {
                PrintSolution(node);
                return -1; // Found solution
            }
            int min = int.MaxValue;
            foreach (var move in GetPossibleMoves(node.State))
            {
                int[,] newState = Swap(node.State, move);
                int cost = node.Depth + 1 + CalculateHeuristic(newState);
                Node child = new Node(newState, cost, node.Depth + 1, node);
                int result = DepthLimitedSearch(child, threshold);

                if (result == -1)
                    return -1; // Solution found

                if (result < min)
                    min = result;
            }
            return min;
        }

        // Gets the memoized heuristic value for a state
        private int GetMemoizedHeuristic(int[,] state)
        {
            string stateString = StateToString(state);
            if (memoizedHeuristics.TryGetValue(stateString, out int cachedHeuristic))
            {
                return cachedHeuristic;
            }
            return -1; // Return a value indicating that the heuristic is not memoized
        }

        // Memoizes the heuristic value for a state
        private void MemoizeHeuristic(int[,] state, int heuristicValue)
        {
            string stateString = StateToString(state);
            memoizedHeuristics[stateString] = heuristicValue;
        }

        // Calculates the heuristic value for a state
        private int CalculateHeuristic(int[,] state)
        {
            int h = 0;
            int memoizedHeuristic = GetMemoizedHeuristic(state);
            if (memoizedHeuristic != -1)
            {
                return memoizedHeuristic;
            }
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] != goalState1[i, j] && state[i, j] != goalState2[i, j] && state[i, j] != 0)
                        h++;
                }
            }
            MemoizeHeuristic(state, h); // Store the calculated heuristic value
            return h;
        }

        // Checks if a state is the goal state
        private bool IsGoalState(int[,] state)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] != goalState1[i, j] && state[i, j] != goalState2[i, j])
                        return false;
                }
            }
            return true;
        }

        // Generates possible moves for the empty tile
        private IEnumerable<int[]> GetPossibleMoves(int[,] state)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (state[row, col] == 0)
                    {
                        emptyRow = row;
                        emptyCol = col;
                    }
                }
            }
            foreach (var direction in directions)
            {
                int newI = emptyRow + direction / state.GetLength(1);
                int newJ = emptyCol + direction % state.GetLength(1);

                if (newI >= 0 && newI < state.GetLength(0) && newJ >= 0 && newJ < state.GetLength(1))
                    yield return new int[] { newI, newJ };
            }
        }

        // Swaps the empty tile with a neighboring tile
        private int[,] Swap(int[,] state, int[] newIndex)
        {
            int[,] newState = (int[,])state.Clone();
            int zeroIndexI = -1;
            int zeroIndexJ = -1;

            for (int i = 0; i < newState.GetLength(0); i++)
            {
                for (int j = 0; j < newState.GetLength(1); j++)
                {
                    if (newState[i, j] == 0)
                    {
                        zeroIndexI = i;
                        zeroIndexJ = j;
                        break;
                    }
                }
                if (zeroIndexI != -1)
                    break;
            }
            newState[zeroIndexI, zeroIndexJ] = newState[newIndex[0], newIndex[1]];
            newState[newIndex[0], newIndex[1]] = 0;
            return newState;
        }

        // Prints the solution path
        private void PrintSolution(Node node)
        {
            List<Node> path = new List<Node>();
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            for (int i = path.Count - 1; i >= 0; i--)
            {
                if (path.Count - i - 1 == 0)
                {
                    Console.WriteLine($"Initial State:");
                    PrintState(path[i].State);
                }
                else
                {
                    Console.WriteLine($"Step {path.Count - i - 1}:");
                    PrintState(path[i].State);
                }
            }
        }

        // Prints the current state of the puzzle
        private void PrintState(int[,] state)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    Console.Write(state[i, j] == 0 ? "  " : state[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}