namespace AILibraryForNPC.core
{
    using System.Collections.Generic;
    using UnityEngine;

    public class GoalSystem : MonoBehaviour
    {
        [SerializeField]
        private List<BaseGoalSO> goals = new List<BaseGoalSO>();

        [SerializeField]
        private IGoalSelector goalSelector;

        public BaseGoalSO SelectBestGoal(WorldState worldState)
        {
            // return goalSelector.SelectBestGoal(goals, worldState);
            return goals[0];
        }
    }
}
