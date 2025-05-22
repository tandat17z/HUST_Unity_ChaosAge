using AILibraryForNPC.Core;
using ChaosAge.AI.battle;
using ChaosAge.Battle;
using UnityEngine;

public class ATSensor : BaseSensor_v2
{
    public override void Observe(WorldState_v2 worldstate)
    {
        BattleUnit minUnit = null;
        float minDistance = int.MaxValue;
        foreach (var unit in AIBattleManager.Instance.units)
        {
            var distance = Vector3.Distance(unit.transform.position, agent.transform.position);
            if (minUnit == null || distance < minDistance)
            {
                minUnit = unit;
                minDistance = distance;
            }
        }
        // worldstate.AddState("Unit", minUnit);
        worldstate.AddBuffer("TargetUnit", minUnit);
    }
}
