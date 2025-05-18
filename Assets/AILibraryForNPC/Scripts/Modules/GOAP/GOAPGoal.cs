using AILibraryForNPC.Core;

namespace AILibraryForNPC.GOAP
{
    public abstract class GOAPGoal
    {
        public abstract bool IsGoalReached(WorldState_v2 worldState);
        public abstract float GetWeight(WorldState_v2 worldState);
    }
}
