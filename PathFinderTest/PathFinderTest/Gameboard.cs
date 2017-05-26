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

        /// <summary>
        /// Gameboard constructor
        /// </summary>
        /// <param name="nodeMap">Node map to search through</param>
        /// <param name="startNode">Starting node</param>
        /// <param name="endNode">Ending node</param>
        /// <param name="wallsList">list of walls</param>
        public Gameboard(Node[,] nodeMap, Node startNode, Node endNode, List<Node> wallsList = null)
        {
            NodeMap = nodeMap;
            StartNode = NodeMap[startNode.X, startNode.Y];
            EndNode = NodeMap[endNode.X, endNode.Y];
            StartNode.PrintState = NodeState.Start;
            EndNode.PrintState = NodeState.End;
            Walls = wallsList;
            AddWalls(wallsList);
        }

        /// <summary>
        /// Add walls to the nodemap
        /// </summary>
        /// <param name="walls">list of walls to be added</param>
        private void AddWalls(IEnumerable<Node> walls)
        {
            if (walls == null)
            {
                return;
            }
            foreach (var wall in walls)
            {
                NodeMap[wall.X, wall.Y].PrintState = NodeState.Wall;
            }
        }
    }
}
