using System;
using System.Collections.Generic;

namespace AILibraryForNPC.Algorithms
{
    public class AStar
    {
        public static List<INode> FindPath(INode start, INode goal)
        {
            var openSet = new List<INode>();
            var closedSet = new List<INode>();
            var cameFrom = new Dictionary<INode, INode>();
            var gScore = new Dictionary<INode, float>();
            var fScore = new Dictionary<INode, float>();

            openSet.Add(start);
            gScore[start] = 0;
            fScore[start] = start.GetHeuristic(goal);

            while (openSet.Count > 0)
            {
                var current = GetLowestFScore(openSet, fScore);

                if (current.Equals(goal))
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in current.GetNeighbors())
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    var tentativeGScore = gScore[current] + current.GetCost(neighbor);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else if (tentativeGScore >= gScore[neighbor])
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + neighbor.GetHeuristic(goal);
                }
            }
            return null;
        }

        private static List<INode> ReconstructPath(Dictionary<INode, INode> cameFrom, INode current)
        {
            var path = new List<INode>();
            while (current != null)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Reverse();
            return path;
        }

        private static INode GetLowestFScore(List<INode> openSet, Dictionary<INode, float> fScore)
        {
            var lowest = openSet[0];
            foreach (var inode in openSet)
            {
                if (fScore[inode] < fScore[lowest])
                    lowest = inode;
            }
            return lowest;
        }
    }
}
