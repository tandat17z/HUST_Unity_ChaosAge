using System.Collections.Generic;
using AILibraryForNPC.core.Modules.GOAP;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public class GOAPNode
    {
        public GOAPNode parent;
        public float runningCost;
        public float heuristicCost;
        public GOAPAction action;
        public Dictionary<string, int> state;

        public GOAPNode(
            GOAPNode parent,
            float runningCost,
            Dictionary<string, int> state,
            GOAPAction action
        )
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = new Dictionary<string, int>(state);
            this.action = action;
        }

        public float GetTotalCost()
        {
            return runningCost + heuristicCost;
        }
    }
}
