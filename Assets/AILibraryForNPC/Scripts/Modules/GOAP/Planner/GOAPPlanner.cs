using System.Collections.Generic;
using AILibraryForNPC.Algorithms;
using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public enum PathfindingAlgorithm
    {
        AStar,
        Dijkstra,
    }

    public class GOAPPlanner
    {
        private PathfindingAlgorithm currentAlgorithm = PathfindingAlgorithm.AStar;

        public void SetAlgorithm(PathfindingAlgorithm algorithm)
        {
            currentAlgorithm = algorithm;
        }

        public Queue<GOAPAction> Plan(
            List<GOAPAction> actions,
            Dictionary<string, float> goal,
            WorldState_v2 worldState
        )
        {
            var start = new GOAPNode(worldState.GetStates(), actions);
            var target = new GOAPNode(goal, actions);
            List<INode> path = null;

            switch (currentAlgorithm)
            {
                case PathfindingAlgorithm.AStar:
                    path = AStar.FindPath(start, target);
                    break;
                case PathfindingAlgorithm.Dijkstra:
                    path = Dijkstra.FindPath(start, target);
                    break;
            }

            if (path == null)
            {
                Debug.Log("No plan found");
                return null;
            }

            // Convert path to action queue
            var queue = new Queue<GOAPAction>();
            foreach (var node in path)
            {
                if (((GOAPNode)node).action != null)
                {
                    queue.Enqueue(((GOAPNode)node).action);
                }
            }

            return queue;
        }
    }
}
