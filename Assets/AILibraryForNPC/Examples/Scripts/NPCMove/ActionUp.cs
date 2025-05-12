using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using ChaosAge.AI.battle;
using UnityEngine;

public class ActionUp : GOAPAction
{
    private float stepTime;

    public override void ApplyEffect(Dictionary<string, float> state)
    {
        state["locationY"] += 1;

        var pos = $"{state["locationX"]}_{state["locationY"]}";
        state[pos] = 1;
    }

    public override bool CheckPrecondition(Dictionary<string, float> state)
    {
        var nextX = state["locationX"];
        var nextY = state["locationY"] + 1;
        var pos = $"{nextX}_{nextY}";
        if (state[pos] != 0 || AIBattleManager.Instance.CanMove(nextX, nextY) == false)
        {
            return false;
        }
        return true;
    }

    public override float GetCost()
    {
        return 1;
    }

    public override Dictionary<string, float> GetEffect()
    {
        var eff = new Dictionary<string, float>();
        eff.Add("up", 1);
        eff.Add("preActionUp", 1);
        return eff;
    }

    public override Dictionary<string, float> GetPrecondition()
    {
        var eff = new Dictionary<string, float>();
        eff.Add("preActionDown", 0);
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
        agent.transform.position += new Vector3(0, 0, 1);
    }

    public override void PrePerform(WorldState_v2 worldState)
    {
        stepTime = 1;
    }
}
