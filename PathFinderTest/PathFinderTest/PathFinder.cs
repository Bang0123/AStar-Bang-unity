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

        /// <summary>
        /// Constructor for pathfinder object
        /// </summary>
        /// <param name="gb">Gameboard Object with all prop filled</param>
        public PathFinder(Gameboard gb)
        {
            Gameboard = gb;
            Gameboard.StartNode.State = NodeState.Open;
        }

        /// <summary>
        /// Start A* search algorithm
        /// </summary>
        public void FindAStarPath()
        {
            float cost = 0;
            if (SearchAStarPath(Gameboard.StartNode))
            {
                cost += BackTrack(Gameboard.EndNode);
            }
            Console.WriteLine($"Total cost of route: {cost}");
        }

        /// <summary>
        /// Search for end from given node
        /// </summary>
        /// <param name="location"></param>
        /// <returns>returns true if solution found for node else dead end</returns>
        private bool SearchAStarPath(Node location)
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
                if (SearchAStarPath(node))
                {
                    return true;
                }
            }
            // No solution
            return false;
        }

        /// <summary>
        /// Backtracks to make it visible what path A* calculated 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private float BackTrack(Node node)
        {
            var endnode = node;
            while (node.ParentNode != null)
            {
                node.PrintState = NodeState.Walked;
                node = node.ParentNode;
            }
            endnode.PrintState = NodeState.End;
            return endnode.G;
        }

        /// <summary>
        /// Get All adjacent nodes if they can be traversed
        /// </summary>
        /// <param name="location">Current node</param>
        /// <returns>List of nodes that can be traversed</returns>
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

        /// <summary>
        /// Get All adjacent nodes if they exist with gameboard
        /// </summary>
        /// <param name="location">Location from where you want adjacent nodes</param>
        /// <returns></returns>
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
