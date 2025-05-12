using System.Collections.Generic;
using AILibraryForNPC.Modules.GOAP;

namespace AILibraryForNPC.Algorithms
{
    public class Dijkstra
    {
        public static List<GOAPNode> FindPath(
            GOAPNode start,
            Dictionary<string, float> goal,
            List<GOAPAction> availableActions
        )
        {
            var unvisited = new List<GOAPNode>();
            var visited = new HashSet<GOAPNode>();
            var distances = new Dictionary<GOAPNode, float>();

            unvisited.Add(start);
            distances[start] = 0;

            while (unvisited.Count > 0)
            {
                var current = GetLowestDistance(unvisited, distances);

                if (GoalAchieved(goal, current.state))
                {
                    return ReconstructPath(current);
                }

                unvisited.Remove(current);
                visited.Add(current);

                foreach (var action in availableActions)
                {
                    if (!action.CheckPrecondition(current.state))
                        continue;

                    var newState = new Dictionary<string, float>(current.state);
                    action.ApplyEffect(newState);

                    var neighbor = new GOAPNode(
                        current,
                        current.runningCost + action.GetCost(),
                        newState,
                        action
                    );

                    if (visited.Contains(neighbor))
                        continue;

                    var distance = distances[current] + action.GetCost();

                    if (!unvisited.Contains(neighbor))
                        unvisited.Add(neighbor);
                    else if (distance >= distances[neighbor])
                        continue;

                    distances[neighbor] = distance;
                }
            }

            return null; // No path found
        }

        private static GOAPNode GetLowestDistance(
            List<GOAPNode> unvisited,
            Dictionary<GOAPNode, float> distances
        )
        {
            GOAPNode lowest = unvisited[0];
            float lowestDistance = distances[lowest];

            foreach (var node in unvisited)
            {
                if (distances[node] < lowestDistance)
                {
                    lowest = node;
                    lowestDistance = distances[node];
                }
            }

            return lowest;
        }

        private static List<GOAPNode> ReconstructPath(GOAPNode current)
        {
            var path = new List<GOAPNode>();
            while (current != null)
            {
                path.Insert(0, current);
                current = current.parent;
            }
            return path;
        }

        private static bool GoalAchieved(
            Dictionary<string, float> goal,
            Dictionary<string, float> currentState
        )
        {
            foreach (var goalItem in goal)
            {
                if (!currentState.ContainsKey(goalItem.Key))
                    return false;
                if (currentState[goalItem.Key] < goalItem.Value)
                    return false;
            }
            return true;
        }
    }
}
