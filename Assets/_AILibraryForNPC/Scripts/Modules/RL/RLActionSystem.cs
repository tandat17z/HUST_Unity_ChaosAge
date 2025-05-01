using System.Collections.Generic;
using AILibraryForNPC.core;
using UnityEngine;

namespace AILibraryForNPC.Modules.RL
{
    public class RLActionSystem : ActionSystem
    {
        public override void Initialize()
        {
            // Không cần thiết phải khởi tạo gì ở đây
        }

        public override BaseAction GetAction(BaseGoal goal, WorldState worldState)
        {
            // Lấy danh sách các action có thể thực hiện
            var availableActions = GetAvailableActions();
            return availableActions[0];

            // // Nếu có RLAgent, sử dụng Q-learning để chọn action
            // if (TryGetComponent<RLAgent>(out var rlAgent))
            // {
            //     // Chuyển đổi WorldState thành state key
            //     string stateKey = (worldState as RLWorldState)?.GetStateKey();

            //     // Chọn action dựa trên Q-values
            //     int actionIndex = rlAgent.ChooseAction(worldState);
            //     if (actionIndex >= 0 && actionIndex < availableActions.Count)
            //     {
            //         return availableActions[actionIndex];
            //     }
            // }

            // // Fallback: chọn action theo cách thông thường
            // return base.GetAction(goal, worldState);
        }
    }
}
