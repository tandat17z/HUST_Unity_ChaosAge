using System.Collections.Generic;
using AILibraryForNPC.Algorithms;
using AILibraryForNPC.Core;
using AILibraryForNPC.GOAP;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPNode : INode
    {
        public static bool isLog = false;  // Static field to control logging
        public GOAPAction action;
        public WorldState_v2 worldState;
        public List<GOAPAction> availableActions;
        public GOAPBaseGoal goal;

        public GOAPNode(
            WorldState_v2 state,
            List<GOAPAction> availableActions,
            GOAPAction action = null
        )
        {
            this.worldState = state;
            this.availableActions = new List<GOAPAction>(availableActions);
            this.action = action;
        }

        public void SetGoal(GOAPBaseGoal goal)
        {
            this.goal = goal;
        }

        public bool Equals(INode other)
        {
            var otherNode = (GOAPNode)other;
            return worldState.Equals(otherNode.worldState);
        }

        public float GetCost()
        {
            return action.GetCost();
        }

        public float GetHeuristic(INode node)
        {
            return goal.GetHeuristic((node as GOAPNode).worldState);
        }

        public List<INode> GetNeighbors()
        {
            var neighbors = new List<INode>();

            if (isLog)
            {
                Debug.LogWarning("current node: " + worldState.GetStateKey());
            }
            foreach (var action in availableActions)
            {
                if (action.CheckPrecondition(worldState))
                {
                    var newState = worldState.Clone();
                    action.ApplyEffect(newState);
                    neighbors.Add(new GOAPNode(newState, availableActions, action));
                    if (isLog)
                    {
                        Debug.Log(
                            "GetNeighbors: " + action.GetType().Name + " " + newState.GetString()
                        );
                    }
                }
            }
            return neighbors;
        }

        public bool CheckIfGoalReached(INode current)
        {
            return goal.IsGoalReached((current as GOAPNode).worldState);
        }
    }
}
