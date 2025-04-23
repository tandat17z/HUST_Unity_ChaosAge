using System.Collections.Generic;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public class GOAPActionSystem : ActionSystem
    {
        private GOAPPlanner planner;
        private List<GOAPAction> availableActions = new List<GOAPAction>();
        private Queue<GOAPAction> actionQueue;

        public override void Initialize()
        {
            planner = (agent as GOAPAgent).planner;
            availableActions.AddRange(GetComponents<GOAPAction>());
        }

        public override BaseAction GetAction(BaseGoal goal, WorldState worldState)
        {
            var goapGoal = goal as GOAPGoal;
            // Lên kế hoạch action mới nếu cần
            if (actionQueue == null || actionQueue.Count == 0)
            {
                // Kiểm tra xem goal có hợp lệ không
                // if (!goal.IsValid(worldState))
                //     be;

                // Tạo kế hoạch mới
                actionQueue = planner.Plan(availableActions, goapGoal.GetTargetState(), worldState);
            }

            // Lấy action tiếp theo từ plan
            if (actionQueue != null && actionQueue.Count > 0)
            {
                var currentAction = actionQueue.Dequeue();
                (currentAction as GOAPAction)?.PrePerform();
                return currentAction;
            }
            return null;
        }
    }
}
