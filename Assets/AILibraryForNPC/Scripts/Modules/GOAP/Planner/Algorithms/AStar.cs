using System.Collections.Generic;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;

namespace AILibraryForNPC.Algorithms
{
    public class AStar
    {
        public static List<GOAPNode> FindPath(
            GOAPNode start,
            Dictionary<string, float> goal,
            List<GOAPAction> availableActions
        )
        {
            var openSet = new List<GOAPNode>();
            var closedSet = new HashSet<GOAPNode>();
            var cameFrom = new Dictionary<GOAPNode, GOAPNode>();
            var gScore = new Dictionary<GOAPNode, float>();
            var fScore = new Dictionary<GOAPNode, float>();

            openSet.Add(start);
            gScore[start] = 0;
            fScore[start] = Heuristic(start.state, goal);

            while (openSet.Count > 0)
            {
                var current = GetLowestFScore(openSet, fScore);

                if (GoalAchieved(goal, current.state))
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

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

                    if (closedSet.Contains(neighbor))
                        continue;

                    var tentativeGScore = gScore[current] + action.GetCost();

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else if (tentativeGScore >= gScore[neighbor])
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor.state, goal);
                }
            }

            return null; // No path found
        }

        private static float Heuristic(
            Dictionary<string, float> state,
            Dictionary<string, float> goal
        )
        {
            float h = 0;
            foreach (var goalItem in goal)
            {
                if (!state.ContainsKey(goalItem.Key))
                {
                    h += goalItem.Value;
                }
                else if (state[goalItem.Key] < goalItem.Value)
                {
                    h += goalItem.Value - state[goalItem.Key];
                }
            }
            return h;
        }

        private static GOAPNode GetLowestFScore(
            List<GOAPNode> openSet,
            Dictionary<GOAPNode, float> fScore
        )
        {
            GOAPNode lowest = openSet[0];
            float lowestScore = fScore[lowest];

            foreach (var node in openSet)
            {
                if (fScore[node] < lowestScore)
                {
                    lowest = node;
                    lowestScore = fScore[node];
                }
            }

            return lowest;
        }

        private static List<GOAPNode> ReconstructPath(
            Dictionary<GOAPNode, GOAPNode> cameFrom,
            GOAPNode current
        )
        {
            var path = new List<GOAPNode>();
            while (current != null)
            {
                path.Insert(0, current);
                cameFrom.TryGetValue(current, out current);
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
