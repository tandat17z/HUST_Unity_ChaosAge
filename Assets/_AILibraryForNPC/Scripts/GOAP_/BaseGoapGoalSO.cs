using AILibraryForNPC.core;

public abstract class BaseGoapGoalSO : BaseGoalSO
{
    public GOAPPlanner planner;

    public void Initialize(IHeuristic heuristic)
    {
        planner = new GOAPPlanner(heuristic);
    }
}
