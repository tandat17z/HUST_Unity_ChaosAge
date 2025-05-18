using AILibraryForNPC.Core;
using ChaosAge.AI.battle;
using ChaosAge.Config;

public class GoapBuildingSensor : BaseSensor_v2
{
    public override void Observe(WorldState_v2 worldstate)
    {
        var buildings = AIBattleManager.Instance.buildings;

        worldstate.AddBuffer("Townhall", null);
        worldstate.AddBuffer("Defense", null);
        foreach (var building in buildings)
        {
            if (building.type == EBuildingType.townhall && worldstate.GetBuffer("Townhall") == null)
            {
                worldstate.AddState("TownhallHp", building.health);
                worldstate.AddBuffer("Townhall", building.gameObject);
            }
            else if (
                building.type == EBuildingType.archertower
                && worldstate.GetBuffer("Defense") == null
            )
            {
                worldstate.AddState("DefenseHp", building.health);
                worldstate.AddBuffer("Defense", building.gameObject);
            }
        }
    }
}
