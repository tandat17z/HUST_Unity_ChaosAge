namespace AILibraryForNPC.core
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class BaseGoalSO : ScriptableObject
    {
        [SerializeField]
        protected List<BaseActionSO> availableActions = new List<BaseActionSO>();

        // Tạo kế hoạch thực hiện mục tiêu
        public abstract List<BaseActionSO> CreatePlan(WorldState state);
    }
}
