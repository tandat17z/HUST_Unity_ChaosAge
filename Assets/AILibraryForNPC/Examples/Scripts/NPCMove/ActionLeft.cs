using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;

public class ActionLeft : GOAPAction
{
    private float stepTime;

    public override float GetCost()
    {
        return 1;
    }

    public override Dictionary<string, float> GetEffect()
    {
        var eff = new Dictionary<string, float>();
        eff.Add("left", 1);
        eff.Add("preActionLeft", 1);
        return eff;
    }

    public override Dictionary<string, float> GetPrecondition()
    {
        var eff = new Dictionary<string, float>();
        eff.Add("preActionRight", 0);
        return eff;
    }

    public override bool IsComplete(WorldState_v2 worldState)
    {
        return stepTime <= 0;
    }

    public override void Perform(WorldState_v2 worldState)
    {
        stepTime -= Time.deltaTime;
    }

    public override void PostPerform(WorldState_v2 worldState)
    {
        agent.transform.position += new Vector3(-1, 0, 0);
    }

    public override void PrePerform(WorldState_v2 worldState)
    {
        stepTime = 1;
    }
}
