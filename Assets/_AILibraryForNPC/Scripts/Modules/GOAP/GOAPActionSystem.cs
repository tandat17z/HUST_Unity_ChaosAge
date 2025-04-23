using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public class GOAPActionSystem : ActionSystem
    {
        private GOAPPlanner planner;
        private Queue<GOAPAction> actionQueue;

        public override void Initialize()
        {
            planner = new GOAPPlanner();
        }

        public override BaseAction GetAction(BaseGoal goal, WorldState worldState)
        {
            var goapGoal = goal as GOAPGoal;
            // Lên kế hoạch action mới nếu cần
            if (actionQueue == null || actionQueue.Count == 0)
            {
                // Tạo kế hoạch mới
                actionQueue = planner.Plan(availableActions, goapGoal.GetTargetState(), worldState);
            }

            // Lấy action tiếp theo từ plan
            if (actionQueue != null && actionQueue.Count > 0)
            {
                var currentAction = actionQueue.Dequeue();
                (currentAction as GOAPAction)?.PrePerform(worldState);
                return currentAction;
            }
            return null;
        }
    }
}
