using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    [RequireComponent(typeof(GOAPActionSystem))]
    [RequireComponent(typeof(GOAPGoalSystem))]
    public abstract class GOAPAgent : BaseAgent
    {
        public GOAPGoalSystem goalSystem => GetComponent<GOAPGoalSystem>();

        public abstract void RegisterGoals();

        public void CancelPlan()
        {
            (currentAction as GOAPAction).Cancel();
            currentAction = null;

            (actionSystem as GOAPActionSystem).CancelPlan();
        }
    }
}
