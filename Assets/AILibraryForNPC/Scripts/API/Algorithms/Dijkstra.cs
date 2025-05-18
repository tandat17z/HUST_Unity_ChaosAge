using System;
using System.Collections.Generic;

namespace AILibraryForNPC.Algorithms
{
    public class Dijkstra
    {
        public static List<INode> FindPath(INode start, INode goal)
        {
            var unvisited = new List<INode>();
            var cameFrom = new Dictionary<INode, INode>();
            var visited = new HashSet<INode>();
            var distances = new Dictionary<INode, float>();

            unvisited.Add(start);
            distances[start] = 0;

            while (unvisited.Count > 0)
            {
                var current = GetLowestDistance(unvisited, distances);

                if (current.Equals(goal))
                {
                    return ReconstructPath(cameFrom, current);
                }

                unvisited.Remove(current);
                visited.Add(current);

                foreach (var neighbor in current.GetNeighbors())
                {
                    if (IsContains(visited, neighbor))
                        continue;

                    var tentativeDistance = distances[current] + current.GetCost(neighbor);

                    if (!IsContains(unvisited, neighbor))
                        unvisited.Add(neighbor);
                    else if (tentativeDistance >= distances[neighbor])
                        continue;

                    distances[neighbor] = tentativeDistance;
                    cameFrom[neighbor] = current;
                }
            }
            return null;
        }

        private static List<INode> ReconstructPath(Dictionary<INode, INode> cameFrom, INode current)
        {
            var path = new List<INode>();
            while (current != null)
            {
                path.Insert(0, current);
                current = cameFrom[current];
            }
            return path;
        }

        private static INode GetLowestDistance(
            List<INode> unvisited,
            Dictionary<INode, float> distances
        )
        {
            var lowest = unvisited[0];
            foreach (var node in unvisited)
            {
                if (distances[node] < distances[lowest])
                    lowest = node;
            }
            return lowest;
        }

        private static bool IsContains(HashSet<INode> list, INode node)
        {
            foreach (var item in list)
            {
                if (item.Equals(node))
                    return true;
            }
            return false;
        }

        private static bool IsContains(List<INode> list, INode node)
        {
            foreach (var item in list)
            {
                if (item.Equals(node))
                    return true;
            }
            return false;
        }
    }
}
