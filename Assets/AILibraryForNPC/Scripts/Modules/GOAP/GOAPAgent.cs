using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    [RequireComponent(typeof(GOAPActionSystem))]
    [RequireComponent(typeof(GOAPGoalSystem))]
    public abstract class GOAPAgent : BaseAgent
    {
        public GOAPGoalSystem goalSystem => GetComponent<GOAPGoalSystem>();

        public override void OnAwake()
        {
            RegisterGoals();
        }

        public abstract void RegisterGoals();

        public abstract bool ConditionCancelPlan(WorldState_v2 worldState);

        private void CancelPlan()
        {
            if (currentAction != null)
            {
                (currentAction as GOAPAction).Cancel();
            }
            currentAction = null;

            (actionSystem as GOAPActionSystem).CancelPlan();
        }

        protected override void UpdateActionSystem()
        {
            if (ConditionCancelPlan(worldState))
            {
                CancelPlan();
            }
            base.UpdateActionSystem();
        }
    }
}
