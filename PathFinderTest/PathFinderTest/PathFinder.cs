using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTest
{
    public class PathFinder
    {
        private Gameboard Gameboard { get; }

        public PathFinder(Gameboard gb)
        {
            Gameboard = gb;
            Gameboard.StartNode.State = NodeState.Open;
        }

        public void FindPath()
        {
            var cPoint = Gameboard.NodeMap[Gameboard.StartNode.X, Gameboard.StartNode.Y];
            var spResult = SearchPath(cPoint);
            float cost = 0;
            if (spResult)
            {
                cost += BackTrack(Gameboard.EndNode);
            }
            Console.WriteLine("Total cost of route: " + cost);
        }

        private bool SearchPath(Node location)
        {
            location.State = NodeState.Closed;
            var adjecents = GetWalkableAdjacentLocations(location);
            adjecents.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var node in adjecents)
            {
                if (node.PrintState == NodeState.End)
                {
                    return true;
                }
                else
                {
                    if (SearchPath(node))
                    {
                        return true;
                    }
                }
            }
            // No solution
            return false;
        }

        private float BackTrack(Node node)
        {
            var endCost = node.G;
            while (node.ParentNode != null)
            {
                node.PrintState = NodeState.Walked;
                node = node.ParentNode;
            }
            return endCost;
        }

        private List<Node> GetWalkableAdjacentLocations(Node location)
        {
            var adjecents = GetAdjacentLocations(location).Where(x => x.PrintState != NodeState.Wall && x.State != NodeState.Closed).ToList();
            var rList = new List<Node>();
            foreach (var node in adjecents)
            {
                if (node.State == NodeState.Open)
                {
                    var gCost = location.G + Node.GetTraversalCost(node.Point, location.Point);
                    if (gCost < node.G)
                    {
                        node.ParentNode = location;
                        rList.Add(node);
                    }
                }
                else
                {
                    node.ParentNode = location;
                    node.State = NodeState.Open;
                    rList.Add(node);
                }
                if (node.H == 0)
                {
                    node.H = Node.GetTraversalCost(node.Point, Gameboard.EndNode.Point);
                }
            }
            return rList;
        }

        private IEnumerable<Node> GetAdjacentLocations(Node location)
        {
            var nl = new List<Node>();
            try
            {
                nl.Add(Gameboard.NodeMap[location.X - 1, location.Y - 1]);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                nl.Add(Gameboard.NodeMap[location.X - 1, location.Y]);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                nl.Add(Gameboard.NodeMap[location.X - 1, location.Y + 1]);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                nl.Add(Gameboard.NodeMap[location.X, location.Y + 1]);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                nl.Add(Gameboard.NodeMap[location.X + 1, location.Y + 1]);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                nl.Add(Gameboard.NodeMap[location.X + 1, location.Y]);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                nl.Add(Gameboard.NodeMap[location.X + 1, location.Y - 1]);
            }
            catch (Exception)
            {
                // ignored
            }
            try
            {
                nl.Add(Gameboard.NodeMap[location.X, location.Y - 1]);
            }
            catch (Exception)
            {
                // ignored
            }
            return nl;
        }
    }
}
