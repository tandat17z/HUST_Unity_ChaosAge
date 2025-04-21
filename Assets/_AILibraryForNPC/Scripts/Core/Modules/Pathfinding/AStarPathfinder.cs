// Core/Pathfinding/AStarPathfinder.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.Pathfinding
{
    public class AStarPathfinder<T>
        where T : INode
    {
        public static AStarPathfinder<T> Instance;

        private void Awake()
        {
            Instance = this;
        }

        private class NodeRecord
        {
            public T Node { get; set; }
            public NodeRecord Parent { get; set; }
            public float CostSoFar { get; set; }
            public float EstimatedTotalCost { get; set; }
        }

        public List<T> FindPath(T start, T goal, IGraph<T> graph)
        {
            if (!start.IsWalkable || !goal.IsWalkable)
                return null;

            var openList = new List<NodeRecord>();
            var closedList = new HashSet<T>();

            var startRecord = new NodeRecord
            {
                Node = start,
                Parent = null,
                CostSoFar = 0,
                EstimatedTotalCost = graph.GetHeuristic(start, goal),
            };

            openList.Add(startRecord);

            while (openList.Count > 0)
            {
                var current = GetLowestCostNode(openList);

                if (current.Node.Equals(goal))
                {
                    return ReconstructPath(current);
                }

                openList.Remove(current);
                closedList.Add(current.Node);

                foreach (var neighbor in graph.GetNeighbors(current.Node))
                {
                    if (closedList.Contains(neighbor) || !neighbor.IsWalkable)
                        continue;

                    float newCost = current.CostSoFar + graph.GetCost(current.Node, neighbor);
                    var neighborRecord = openList.FirstOrDefault(r => r.Node.Equals(neighbor));

                    if (neighborRecord == null)
                    {
                        neighborRecord = new NodeRecord
                        {
                            Node = neighbor,
                            Parent = current,
                            CostSoFar = newCost,
                            EstimatedTotalCost = newCost + graph.GetHeuristic(neighbor, goal),
                        };
                        openList.Add(neighborRecord);
                    }
                    else if (newCost < neighborRecord.CostSoFar)
                    {
                        neighborRecord.Parent = current;
                        neighborRecord.CostSoFar = newCost;
                        neighborRecord.EstimatedTotalCost =
                            newCost + graph.GetHeuristic(neighbor, goal);
                    }
                }
            }

            return null;
        }

        private NodeRecord GetLowestCostNode(List<NodeRecord> openList)
        {
            return openList.OrderBy(n => n.EstimatedTotalCost).First();
        }

        private List<T> ReconstructPath(NodeRecord endNode)
        {
            var path = new List<T>();
            var current = endNode;

            while (current != null)
            {
                path.Insert(0, current.Node);
                current = current.Parent;
            }

            return path;
        }
    }
}
