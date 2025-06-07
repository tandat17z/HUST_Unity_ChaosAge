using AILibraryForNPC.Core;

public class ATActionSystem : ActionSystem_v2
{
    public override BaseAction_v2 SelectAction(WorldState_v2 worldState)
    {
        if (worldState.GetBuffer("TargetUnit") != null)
        {
            return _actions[0];
        }
        return null;
    }
}
