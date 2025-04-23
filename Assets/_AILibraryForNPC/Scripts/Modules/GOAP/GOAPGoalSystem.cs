using System.Collections.Generic;
using AILibraryForNPC.core;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public class GOAPGoalSystem : GoalSystem
    {
        private List<GOAPGoal> availableGoals = new List<GOAPGoal>();

        protected override void Start()
        {
            base.Start();
            availableGoals.AddRange(GetComponents<GOAPGoal>());
        }

        public override BaseGoal SelectBestGoal(WorldState worldState)
        {
            BaseGoal bestGoal = null;
            float highestPriority = float.MinValue;

            foreach (var goal in availableGoals)
            {
                if (!goal.IsValid(worldState))
                    continue;

                if (goal.priority > highestPriority)
                {
                    highestPriority = goal.priority;
                    bestGoal = goal;
                }
            }

            return bestGoal;
        }
    }
}
