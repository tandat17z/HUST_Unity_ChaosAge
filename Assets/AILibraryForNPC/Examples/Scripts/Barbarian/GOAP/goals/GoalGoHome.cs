using AILibraryForNPC.Core;
using AILibraryForNPC.GOAP;

public class GoalGoHome : GOAPBaseGoal
{
    public override string GetName()
    {
        return "GoalGoHome";
    }

    public override float GetWeight(WorldState_v2 worldState)
    {
        if (worldState.GetState("PlayerHp") <= 50)
        {
            return 1;
        }
        return 0;
    }

    public override bool IsGoalReached(WorldState_v2 worldState)
    {
        return worldState.GetState("home") >= 100;
    }
}
