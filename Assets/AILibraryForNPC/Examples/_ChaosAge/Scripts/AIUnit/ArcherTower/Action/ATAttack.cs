using AILibraryForNPC.Core;
using ChaosAge.Battle;
using ChaosAge.building;
using UnityEngine;

public class ATAttack : BaseAction_v2
{
    private BattleUnit _targetUnit;
    private float _interval;

    public override bool IsComplete(WorldState_v2 worldState)
    {
        return _interval <= 0;
    }

    public override void Perform(WorldState_v2 worldState)
    {
        _interval -= Time.deltaTime;
    }

    public override void PostPerform(WorldState_v2 worldState) { }

    public override void PrePerform(WorldState_v2 worldState)
    {
        _interval = 1;
        _targetUnit = worldState.GetBuffer("TargetUnit") as BattleUnit;

        var damage = GetDamage();
        agent.GetComponent<BattleBuilding>().SpawnProjectile(_targetUnit, damage);
    }

    private float GetDamage()
    {
        var defenseSO = agent.GetComponent<Building>().BuildingConfigSO as DefenseConfigSO;
        return defenseSO.damage;
    }
}
