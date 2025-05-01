using AILibraryForNPC.core;
using AILibraryForNPC.Modules.RL;
using ChaosAge.Config;
using UnityEngine;

public class Attack : BaseAction
{
    public GameObject target;
    public float interval = 1;
    public float countTime = 0;

    public override bool IsActionComplete(WorldState worldState)
    {
        return countTime < 0 || target == null;
    }

    public override void Perform(WorldState worldState)
    {
        if (target == null)
        {
            Debug.LogWarning("Attack: target is null");
            return;
        }

        countTime -= Time.deltaTime;
    }

    public override void PostPerform(WorldState worldState) { }

    public override void PrePerform(WorldState worldState)
    {
        target = (worldState as BarbarianWorldState).GetTarget();
        countTime = interval;

        if (target == null)
        {
            (worldState as BarbarianWorldState).reward -= 3;
            return;
        }

        var building = target.GetComponent<BattleBuilding>();
        if (building != null)
        {
            building.TakeDamage(5);
            var reward = 5;
            if (building.type == EBuildingType.townhall && building.health <= 0)
            {
                (worldState as BarbarianWorldState).reward += 100;
            }
            if (building.type == EBuildingType.archertower && building.health <= 0)
            {
                (worldState as BarbarianWorldState).reward += 50;
            }
            (worldState as BarbarianWorldState).reward += reward;
        }
    }
}
