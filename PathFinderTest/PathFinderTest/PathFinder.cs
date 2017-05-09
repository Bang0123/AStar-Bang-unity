using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTest
{
    public class PathFinder
    {
        public Gameboard Gameboard { get; set; }

        public PathFinder(Gameboard gb)
        {
            this.Gameboard = gb;
        }

        public void FindPath()
        {
            // find the shortest path from node.Start to node.End
            List<Node> pathClosed = new List<Node>();
            var cPoint = Gameboard.NodeMap[Gameboard.StartNode.X, Gameboard.StartNode.Y];
            float cost = 0;
            while (true)
            {
                if (cPoint.X == Gameboard.EndNode.X && cPoint.Y == Gameboard.EndNode.Y)
                {
                    break;
                }
                var currentDistanceToEnd = Node.GetTraversalCost(Gameboard.NodeMap[cPoint.X, cPoint.Y].Point, Gameboard.EndNode.Point);
                cost += currentDistanceToEnd;

                var adjecents = GetWalkableAdjacentLocations(cPoint);

                foreach (var node in adjecents)
                {
                    node.G = Node.GetTraversalCost(node.Point, cPoint.Point);
                    if (node.H == 0)
                    {
                        node.H = Node.GetTraversalCost(node.Point, Gameboard.EndNode.Point);
                    }
                    
                }
                try
                {
                    var closest = adjecents.Min(x => x.F);
                    //adjecents.ToList().Sort((node1, node2) => node1.F.CompareTo(node2.F));
                    var chosenpath = adjecents.First(x => x.F == closest);
                    if (chosenpath.State == NodeState.End)
                    {
                        break;
                    }
                    chosenpath.State = NodeState.Walked;
                    pathClosed.Add(chosenpath);
                    cPoint = chosenpath;
                }
                catch (Exception)
                {
                    break;
                }
            }
            Console.WriteLine("Total cost of route: " + cost);
        }

        private IList<Node> GetWalkableAdjacentLocations(Node location)
        {
            var adjecents = GetAdjacentLocations(location);
            return adjecents.Where(x => x.IsWalkable && x.State != NodeState.Wall && x.State != NodeState.Walked).ToList();
        }


        private IList<Node> GetAdjacentLocations(Node location)
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
