using System.Collections.Generic;
using AILibraryForNPC.Algorithms;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPNode : INode
    {
        public GOAPAction action;
        public Dictionary<string, float> state;
        public List<GOAPAction> availableActions;

        public GOAPNode(Dictionary<string, float> state, List<GOAPAction> availableActions)
        {
            this.state = new Dictionary<string, float>(state);
            this.availableActions = new List<GOAPAction>(availableActions);
        }

        public bool Equals(INode other)
        {
            throw new System.NotImplementedException();
        }

        public float GetCost(INode neighbor)
        {
            throw new System.NotImplementedException();
        }

        public float GetHeuristic(INode goal)
        {
            throw new System.NotImplementedException();
        }

        public List<INode> GetNeighbors()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"State: {string.Join(", ", state)}";
        }
    }
}
