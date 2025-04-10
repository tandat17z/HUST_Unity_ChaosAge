namespace AILibraryForNPC.core
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class ActionSystem : MonoBehaviour
    {
        [SerializeField, ReadOnly] BaseGoalSO goal;

        private Queue<BaseActionSO> queueActions;

        public BaseActionSO GetAction(BaseGoalSO goal, WorldState state)
        {
            if (this.goal != goal || queueActions.Count == 0)
            {
                this.goal = goal;
                queueActions.Clear();
                foreach (var act in goal.CreatePlan(state))
                {
                    queueActions.Enqueue(act);
                }
            }
            return queueActions.Dequeue();
        }
    }
}

