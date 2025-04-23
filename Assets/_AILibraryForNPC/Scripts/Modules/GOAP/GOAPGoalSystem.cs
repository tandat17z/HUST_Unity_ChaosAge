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

        public List<GOAPGoal> GetGoals()
        {
            return availableGoals;
        }

        public override BaseGoal SelectBestGoal(WorldState worldState)
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

    public class AttackBuildingGoal : GOAPGoal
    {
        protected override void Initialize()
        {
            targetStates.Add("hasTarget", 1);
            targetStates.Add("isAtTarget", 1);
        }

        public override bool IsValid(WorldState worldState)
        {
            return worldState.GetState<int>("hasTarget") == 1;
        }

        public override bool IsAchieved(WorldState worldState)
        {
            return worldState.GetState<int>("targetDestroyed") == 1;
        }
    }
}
