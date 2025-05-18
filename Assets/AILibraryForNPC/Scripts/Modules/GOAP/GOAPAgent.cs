using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    [RequireComponent(typeof(GOAPActionSystem))]
    [RequireComponent(typeof(GOAPGoalSystem))]
    public abstract class GOAPAgent : BaseAgent
    {
        public GOAPGoalSystem GoalSystem => GetComponent<GOAPGoalSystem>();

        public void CancelPlan() { }
    }
}
