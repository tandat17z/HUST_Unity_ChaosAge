using AILibraryForNPC.Core;
using AILibraryForNPC.GOAP;

public class GoalTownhall : GOAPBaseGoal
{
    public override string GetName()
    {
        return "GoalTownhall";
    }

    public override float GetWeight(WorldState_v2 worldState)
    {
        if (worldState.GetState("TownhallHp") <= 0)
        {
            return -1f;
        }
        if (worldState.GetState("TownhallHp") <= 40)
        {
            return 0.9f;
        }
        return 0.1f;
    }

    public override bool IsGoalReached(WorldState_v2 worldState)
    {
        return worldState.GetState("TownhallHp") <= 0 && worldState.GetState("PlayerHp") > 0;
    }
}
