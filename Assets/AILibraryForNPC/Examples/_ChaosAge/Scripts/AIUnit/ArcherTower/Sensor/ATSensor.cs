using AILibraryForNPC.Core;
using ChaosAge.AI.battle;
using ChaosAge.Battle;
using ChaosAge.building;
using UnityEngine;

public class ATSensor : BaseSensor_v2
{
    public float GetRange()
    {
        var defenseSO = agent.GetComponent<Building>().BuildingConfigSO as DefenseConfigSO;
        return defenseSO.attackRange;
    }
    public override void Observe(WorldState_v2 worldstate)
    {
        BattleUnit minUnit = null;
        float minDistance = int.MaxValue;
        var range = GetRange();
        foreach (var unit in AIBattleManager.Instance.units)
        {
            var distance = Vector3.Distance(unit.transform.position, agent.transform.position);
            if (distance > range)
                continue;
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
