using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;

public class GOAPNPCMove : GOAPAgent
{
    public override void OnAwake() { }

    public override void RegisterActions()
    {
        actionSystem.AddAction(new ActionUp());
        actionSystem.AddAction(new ActionDown());
        actionSystem.AddAction(new ActionLeft());
        actionSystem.AddAction(new ActionRight());
    }

    public override void RegisterGoals()
    {
        throw new System.NotImplementedException();
    }

    public override void RegisterSensors()
    {
        perceptionSystem.AddSensor(new LocationSensor());
    }

    public override bool ConditionCancelPlan(WorldState_v2 worldState)
    {
        return false;
    }
}
