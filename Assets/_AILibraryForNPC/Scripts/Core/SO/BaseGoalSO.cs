namespace AILibraryForNPC.core
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class BaseGoalSO : ScriptableObject
    {
        List<BaseActionSO> actions;

        public abstract List<BaseActionSO> CreatePlan(WorldState state);
    }
}

