using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTest
{
    public class Node
    {
        private Node _lastNode;
        public Point Point { get; }
        public NodeState State { get; set; }
        public NodeState PrintState { get; set; }
        public float G { get; set; }
        public float H { get; set; }
        public Node ParentNode
        {
            get { return _lastNode; }
            set
            {
                _lastNode = value;
                G = _lastNode.G + GetTraversalCost(Point, _lastNode.Point);
            }
        }

        public float F
        {
            get { return G + H; }
        }

        public int X
        {
            get { return Point.X; }
            set { Point.X = value; }
        }

        public int Y
        {
            get { return Point.Y; }
            set { Point.Y = value; }
        }

        public Node(int x, int y)
        {
            Point = new Point(x, y);
            State = NodeState.Untested;
        }

        public static float GetTraversalCost(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
