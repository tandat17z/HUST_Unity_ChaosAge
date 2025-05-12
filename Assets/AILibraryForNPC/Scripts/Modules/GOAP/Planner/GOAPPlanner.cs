using System;
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
            var start = new GOAPNode(null, 0, worldState.GetStates(), null);
            List<GOAPNode> path = null;

            switch (currentAlgorithm)
            {
                case PathfindingAlgorithm.AStar:
                    path = AStar.FindPath(start, goal, actions);
                    break;
                case PathfindingAlgorithm.Dijkstra:
                    path = Dijkstra.FindPath(start, goal, actions);
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
                if (node.action != null)
                {
                    queue.Enqueue(node.action);
                }
            }

            return queue;
        }
    }
}
