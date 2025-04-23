using AILibraryForNPC.core;
using AILibraryForNPC.core.Modules.GOAP;

public class BuildingAttackGoal : GOAPGoal
{
    public override bool IsValid(WorldState worldState)
    {
        return true;
    }
}
