using System.Collections.Generic;
using AILibraryForNPC.Algorithms;
using AILibraryForNPC.Core;
using AILibraryForNPC.GOAP;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPNode : INode
    {
        public GOAPAction action;
        public WorldState_v2 worldState;
        public Dictionary<string, float> state;
        public List<GOAPAction> availableActions;
        public GOAPGoal goal;

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

        public void SetGoal(GOAPGoal goal)
        {
            this.goal = goal;
        }

        public bool Equals(INode other)
        {
            var otherNode = (GOAPNode)other;
            return worldState.Equals(otherNode.worldState);
        }

        public float GetCost(INode neighbor)
        {
            return action.GetCost();
        }

        public float GetHeuristic(INode goal)
        {
            return 1;
        }

        public List<INode> GetNeighbors()
        {
            var neighbors = new List<INode>();
            foreach (var action in availableActions)
            {
                if (action.CheckPrecondition(worldState))
                {
                    var newState = worldState.Clone();
                    action.ApplyEffect(newState);
                    neighbors.Add(new GOAPNode(newState, availableActions, action));
                }
            }
            return neighbors;
        }

        public override string ToString()
        {
            return $"State: {string.Join(", ", state)}";
        }

        public bool CheckIfGoalReached(INode current)
        {
            return goal.IsGoalReached((current as GOAPNode).worldState);
        }
    }
}
