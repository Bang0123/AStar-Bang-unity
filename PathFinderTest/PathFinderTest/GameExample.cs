using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathFinderTest
{
    public class GameExample
    {
        private int length = 12;
        private int height = 8;
        public Gameboard Gameboard { get; set; }

        // remember that
        private Node startPoint = new Node(2, 2);
        private Node endPoint = new Node(6, 8);
        private List<Node> walls = new List<Node>() { new Node(4, 6), new Node(3, 6), new Node(4, 5),  new Node(2, 6) };


        public GameExample()
        {
            // fill board with empty spaces
            Gameboard = new Gameboard(new Node[height, length], startPoint, endPoint);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Gameboard.NodeMap[i, j] = new Node(i, j) { State = NodeState.Path };
                }
            }

            // add walls
            foreach (var wall in walls)
            {
                Gameboard.NodeMap[wall.X, wall.Y].State = NodeState.Wall;
                Gameboard.NodeMap[wall.X, wall.Y].IsWalkable = false;
            }

            // add start and end points
            Gameboard.NodeMap[startPoint.X, startPoint.Y].State = NodeState.Start;
            Gameboard.NodeMap[endPoint.X, endPoint.Y].State = NodeState.End;
            var pathfinder = new PathFinder(Gameboard);
            pathfinder.FindPath();
        }


        

        

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (Gameboard.NodeMap[i, j].State == NodeState.Path) stringBuilder.Append((char)183);
                    if (Gameboard.NodeMap[i, j].State == NodeState.Wall) stringBuilder.Append((char)219);
                    if (Gameboard.NodeMap[i, j].State == NodeState.Start) stringBuilder.Append("S");
                    if (Gameboard.NodeMap[i, j].State == NodeState.End) stringBuilder.Append("E");
                    if (Gameboard.NodeMap[i, j].State == NodeState.Walked) stringBuilder.Append("W");
                }
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();

        }
    }
}
