using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP
{
    [RequireComponent(typeof(GOAPGoalSystem))]
    [RequireComponent(typeof(GOAPActionSystem))]
    public class GOAPAgent : Agent
    {
        public GOAPPlanner planner;

        public override void Initialize()
        {
            planner = new GOAPPlanner();
        }

        //         private void UpdateGOAPDecisionMaking(WorldState worldState)
        //         {
        //             // Kiểm tra action hiện tại
        //             if (currentAction != null)
        //             {
        //                 var goapAction = currentAction as GOAPAction;
        //                 if (goapAction != null && !goapAction.IsActionComplete())
        //                 {
        //                     goapAction.Perform();
        //                     return;
        //                 }
        //                 else
        //                 {
        //                     goapAction?.PostPerform();
        //                     currentAction = null;
        //                 }
        //             }

        //             // Lên kế hoạch action mới nếu cần
        //             if (actionQueue == null || actionQueue.Count == 0)
        //             {
        //                 var goapGoalSystem = goalSystem as GOAPGoalSystem;
        //                 var goapActionSystem = actionSystem as GOAPActionSystem;

        //                 if (goapGoalSystem != null && goapActionSystem != null)
        //                 {
        //                     // Chọn goal tốt nhất
        //                     var sortedGoals = goapGoalSystem.GetGoals().OrderByDescending(x => x.Priority);

        //                     foreach (var goal in sortedGoals)
        //                     {
        //                         // Kiểm tra xem goal có hợp lệ không
        //                         if (!goal.IsValid(worldState))
        //                             continue;

        //                         // Kiểm tra cache
        //                         if (cachedPlans.TryGetValue(goal, out var cachedQueue))
        //                         {
        //                             actionQueue = cachedQueue;
        //                             currentGoal = goal;
        //                             break;
        //                         }

        //                         // Tạo kế hoạch mới
        //                         actionQueue = planner.Plan(
        //                             goapActionSystem.GetActions(),
        //                             goal.GetPreconditions(),
        //                             worldState
        //                         );

        //                         if (actionQueue != null && actionQueue.Count > 0)
        //                         {
        //                             // Cache kế hoạch
        //                             cachedPlans[goal] = new Queue<GOAPAction>(actionQueue);
        //                             currentGoal = goal;
        //                             break;
        //                         }
        //                     }
        //                 }
        //             }

        //             // Lấy action tiếp theo từ plan
        //             if (actionQueue != null && actionQueue.Count > 0)
        //             {
        //                 currentAction = actionQueue.Dequeue();
        //                 (currentAction as GOAPAction)?.PrePerform();
        //             }
        //         }

        //         // Xóa cache khi có thay đổi trong thế giới
        //         public void ClearPlanCache()
        //         {
        //             cachedPlans.Clear();
        //             actionQueue = null;
        //             currentGoal = null;
        //         }
    }
}
