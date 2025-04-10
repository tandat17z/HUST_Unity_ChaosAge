namespace AILibraryForNPC.core
{
    using System.Collections.Generic;

    public interface IGoalSelector
    {
        public BaseGoalSO SelectBestGoal(List<BaseGoalSO> actions, WorldState worldState);
    }
}
