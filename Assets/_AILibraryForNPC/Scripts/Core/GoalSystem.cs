using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public class GoalSystem : MonoBehaviour
    {
        private List<BaseGoal> availableGoals = new List<BaseGoal>();

        protected virtual void Start()
        {
            availableGoals.AddRange(GetComponents<BaseGoal>());
        }

        public virtual BaseGoal SelectBestGoal(WorldState worldState)
        {
            BaseGoal bestGoal = null;
            float highestPriority = float.MinValue;

            foreach (var goal in availableGoals)
            {
                if (!goal.IsValid(worldState))
                    continue;

                if (goal.Priority > highestPriority)
                {
                    highestPriority = goal.Priority;
                    bestGoal = goal;
                }
            }

            return bestGoal;
        }
    }
}
