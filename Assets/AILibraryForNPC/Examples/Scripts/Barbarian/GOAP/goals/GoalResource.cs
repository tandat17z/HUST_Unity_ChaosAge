using AILibraryForNPC.Core;
using AILibraryForNPC.GOAP;

public class GoalDefense : GOAPBaseGoal
{
    public override string GetName()
    {
        return "GoalDefense";
    }

    public override float GetWeight(WorldState_v2 worldState)
    {
        if (worldState.GetState("DefenseHp") <= 0)
        {
            return -1f;
        }
        if (worldState.GetState("DefenseHp") <= 40)
        {
            return 0.5f;
        }
        return 0.2f;
    }

    public override bool IsGoalReached(WorldState_v2 worldState)
    {
        return worldState.GetState("DefenseHp") <= 0 && worldState.GetState("PlayerHp") > 0;
    }
}
