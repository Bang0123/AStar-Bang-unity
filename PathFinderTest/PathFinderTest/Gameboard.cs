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
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }

        public Node[,] NodeMap { get; set; }

        public Gameboard(Node[,] nodeMap, Node startNode, Node endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
            NodeMap = nodeMap;
        }
    }
}
