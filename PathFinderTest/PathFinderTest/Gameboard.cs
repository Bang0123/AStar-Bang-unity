using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTest
{
    public class Gameboard
    {
        public Node StartNode { get; }
        public Node EndNode { get; }
        public Node[,] NodeMap { get; }
        private List<Node> Walls { get; }
        public Gameboard(Node[,] nodeMap, Node startNode, Node endNode, List<Node> wallsList)
        {
            NodeMap = nodeMap;
            StartNode = NodeMap[startNode.X, startNode.Y];
            EndNode = NodeMap[endNode.X, endNode.Y];
            StartNode.PrintState = NodeState.Start;
            EndNode.PrintState = NodeState.End;
            Walls = wallsList;
            AddWalls(wallsList);
        }
        private void AddWalls(IEnumerable<Node> walls)
        {
            foreach (var wall in walls)
            {
                NodeMap[wall.X, wall.Y].PrintState = NodeState.Wall;
            }
        }
    }
}
