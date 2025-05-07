using System.Collections.Generic;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPNode
    {
        public GOAPNode parent;
        public float runningCost;
        public float heuristicCost;
        public GOAPAction action;
        public Dictionary<string, float> state;

        public GOAPNode(
            GOAPNode parent,
            float runningCost,
            Dictionary<string, float> state,
            GOAPAction action
        )
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = new Dictionary<string, float>(state);
            this.action = action;
        }

        public float GetTotalCost()
        {
            return runningCost + heuristicCost;
        }

        public override string ToString()
        {
            return $"State: {string.Join(", ", state)}";
        }
    }
}
