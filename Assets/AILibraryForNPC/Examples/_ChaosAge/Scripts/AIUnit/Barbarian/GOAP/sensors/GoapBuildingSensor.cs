using AILibraryForNPC.Core;
using AILibraryForNPC.Examples;
using ChaosAge.AI.battle;
using ChaosAge.data;
using UnityEngine;

public class GoapBuildingSensor : BaseSensor_v2
{
    public override void Observe(WorldState_v2 worldstate)
    {
        var range = 3f;
        var buildings = AIBattleManager.Instance.buildings;

        worldstate.AddBuffer("Townhall", null);
        worldstate.AddBuffer("Defense", null);
        foreach (var building in buildings)
        {
            if (building.Type == EBuildingType.TownHall && worldstate.GetBuffer("Townhall") == null)
            {
                worldstate.AddState("TownhallHp", building.health);
                worldstate.AddBuffer("Townhall", building.gameObject);

                if (
                    Vector3.Distance(
                        agent.transform.position,
                        building.gameObject.transform.position
                    ) < range
                )
                {
                    worldstate.AddState("PlayerState", (int)PlayerState.MoveToTownhall);
                }
            }
            else if (
                building.Type == EBuildingType.ArcherTowner
                && worldstate.GetBuffer("Defense") == null
            )
            {
                worldstate.AddState("DefenseHp", building.health);
                worldstate.AddBuffer("Defense", building.gameObject);

                if (
                    Vector3.Distance(
                        agent.transform.position,
                        building.gameObject.transform.position
                    ) < range
                )
                {
                    worldstate.AddState("PlayerState", (int)PlayerState.MoveToDefense);
                }
            }
        }
    }
}
