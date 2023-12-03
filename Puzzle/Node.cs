using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle
{
    internal class Node: IComparable<Node>
    {
        public int[,] State { get; }
        public int Cost { get; }
        public int Depth { get; }
        public Node Parent { get; }
        public Node(int[,] state, int cost, int depth, Node parent)
        {
            State = state;
            Cost = cost;
            Depth = depth;
            Parent = parent;
        }
        public int CompareTo(Node other)
        {
            // Compare based on the total cost (g + h)
            return (Cost).CompareTo(other.Cost);
        }
    }
}
