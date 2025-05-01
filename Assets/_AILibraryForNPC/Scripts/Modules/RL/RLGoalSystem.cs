using AILibraryForNPC.core;
using UnityEngine;

namespace AILibraryForNPC.Modules.RL
{
    public class RLGoalSystem : GoalSystem
    {
        [System.Serializable]
        public class RLGoal
        {
            public string goalName;
            public float priority;
            public string[] requiredStates;
            public int[] targetValues;
        }

        [SerializeField]
        private RLGoal[] rlGoals;

        public override BaseGoal SelectBestGoal(WorldState worldState)
        {
            BaseGoal bestGoal = null;
            // float bestPriority = float.MinValue;

            // foreach (var goal in rlGoals)
            // {
            //     if (goal.IsGoalComplete(worldState))
            //         continue;

            //     float priority = goal.priority;

            //     // Tính toán thêm priority dựa trên Q-values nếu cần
            //     if (TryGetComponent<RLAgent>(out var rlAgent))
            //     {
            //         // Có thể thêm logic tính toán priority dựa trên Q-values ở đây
            //     }

            //     if (priority > bestPriority)
            //     {
            //         bestPriority = priority;
            //         bestGoal = goal;
            //     }
            // }

            return bestGoal;
        }
    }
}
