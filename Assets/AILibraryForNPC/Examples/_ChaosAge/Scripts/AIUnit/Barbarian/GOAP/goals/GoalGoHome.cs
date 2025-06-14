using AILibraryForNPC.Core;
using AILibraryForNPC.Examples;
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
        return 0.1f;
    }

    public override bool IsGoalReached(WorldState_v2 worldState)
    {
        return worldState.GetState("PlayerState") == (int)PlayerState.Home;
    }

    public override float GetHeuristic(WorldState_v2 worldState)
    {
        return 1000;
    }
}
