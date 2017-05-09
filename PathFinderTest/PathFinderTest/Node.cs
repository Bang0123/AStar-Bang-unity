using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTest
{
    public class Node
    {
        private Node lastNode;
        public Point Point { get; set; }
        public NodeState State { get; set; }
        public bool IsWalkable { get; set; }
        public float G { get; set; }
        public float H { get; set; }
        public Node ParentNode
        {
            get { return this.lastNode; }
            set
            {
                this.lastNode = value;
                this.G = this.lastNode.G + GetTraversalCost(this.Point, this.lastNode.Point);
            }
        }

        public float F
        {
            get { return this.G + this.H; }
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
            IsWalkable = true;
        }

        internal static float GetTraversalCost(Point location, Point otherLocation)
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
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
