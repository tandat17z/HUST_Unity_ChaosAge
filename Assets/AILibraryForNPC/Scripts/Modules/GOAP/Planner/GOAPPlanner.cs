using System;
using System.Collections.Generic;
using AILibraryForNPC.Core;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPPlanner
    {
        public Queue<GOAPAction> Plan(
            List<GOAPAction> actions,
            Dictionary<string, float> goal,
            WorldState_v2 worldState
        )
        {
            // Khởi tạo các node bắt đầu
            var start = new GOAPNode(null, 0, worldState.GetStates(), null);
            var leaves = new List<GOAPNode>();

            bool success = BuildGraph(start, leaves, actions, goal);

            if (!success)
            {
                // Debug.Log("No plan found");
                return null;
            }

            // Tìm node có chi phí thấp nhất
            GOAPNode cheapest = null;
            foreach (var leaf in leaves)
            {
                if (cheapest == null || leaf.GetTotalCost() < cheapest.GetTotalCost())
                {
                    cheapest = leaf;
                }
            }

            // Xây dựng chuỗi action từ node cuối về node đầu
            var result = new List<GOAPAction>();
            var n = cheapest;
            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action);
                }
                n = n.parent;
            }

            // Chuyển thành queue
            var queue = new Queue<GOAPAction>();
            // Debug.LogWarning("Plan:");
            foreach (var a in result)
            {
                queue.Enqueue(a);
                // Debug.Log(a.name);
            }

            return queue;
        }

        private bool BuildGraph(
            GOAPNode parent,
            List<GOAPNode> leaves,
            List<GOAPAction> actions,
            Dictionary<string, float> goal
        )
        {
            bool foundOne = false;

            // Kiểm tra tất cả các action có thể thực hiện
            foreach (var action in actions)
            {
                var goapAction = action as GOAPAction;
                // Kiểm tra preconditions
                if (!goapAction.IsAchievableGiven(parent.state))
                {
                    continue;
                }

                // Tạo trạng thái mới sau khi thực hiện action
                var currentState = new Dictionary<string, float>(parent.state);
                foreach (var effect in goapAction.GetEffect())
                {
                    currentState[effect.Key] = effect.Value;
                }

                // Tạo node mới
                var node = new GOAPNode(
                    parent,
                    parent.runningCost + goapAction.GetCost(),
                    currentState,
                    goapAction
                );

                // Kiểm tra xem đã đạt được goal chưa
                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    // Nếu chưa đạt goal, thêm vào open list để tiếp tục tìm kiếm
                    List<GOAPAction> subset = ActionSubset(actions, goapAction);
                    foundOne = BuildGraph(node, leaves, subset, goal);
                }
            }

            return foundOne;
        }

        private List<GOAPAction> ActionSubset(List<GOAPAction> actions, GOAPAction goapAction)
        {
            var subset = new List<GOAPAction>();
            foreach (var action in actions)
            {
                if (action != goapAction)
                {
                    subset.Add(action);
                }
            }
            return subset;
        }

        private bool GoalAchieved(
            Dictionary<string, float> goal,
            Dictionary<string, float> currentState
        )
        {
            foreach (var goalItem in goal)
            {
                if (currentState[goalItem.Key] < goalItem.Value)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
