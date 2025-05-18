using AILibraryForNPC.Core;
using AILibraryForNPC.GOAP;

public class GoalDefense : GOAPGoal
{
    public override float GetWeight(WorldState_v2 worldState)
    {
        if (worldState.GetState("DefenseHp") <= 40)
        {
            return 0.5f;
        }
        return 0.1f;
    }

    public override bool IsGoalReached(WorldState_v2 worldState)
    {
        return worldState.GetState("DefenseHp") <= 0;
    }
}
