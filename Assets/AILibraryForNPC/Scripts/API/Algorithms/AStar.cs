using System;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.Algorithms
{
    public class AStar
    {
        public static int maxStep = 1000;

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

            int step = 0;
            while (openSet.Count > 0)
            {
                step++;
                if (step > maxStep)
                {
                    return null;
                }

                var current = GetLowestFScore(openSet, fScore);

                if (goal.CheckIfGoalReached(current))
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in current.GetNeighbors())
                {
                    if (IsContains(closedSet, neighbor))
                        continue;

                    var tentativeGScore = gScore[current] + neighbor.GetCost();

                    if (!IsContains(openSet, neighbor))
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

        private static bool IsContains(List<INode> list, INode node)
        {
            foreach (var item in list)
            {
                if (item.Equals(node))
                    return true;
            }
            return false;
        }

        private static List<INode> ReconstructPath(Dictionary<INode, INode> cameFrom, INode current)
        {
            var path = new List<INode>();
            while (current != null)
            {
                path.Add(current);
                if (cameFrom.ContainsKey(current) == false)
                {
                    current = null;
                }
                else
                {
                    current = cameFrom[current];
                }
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
