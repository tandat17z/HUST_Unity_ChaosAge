using AILibraryForNPC.core.Base;
using ChaosAge.AI.battle;
using ChaosAge.Battle;
using ChaosAge.Config;
using UnityEngine;

namespace AILibraryForNPC.Modules.RL.Sensors
{
    public class BarbarianSensor : BaseSensor
    {
        public override void UpdateSensor()
        {
            // TODO: Implement the logic to update the sensor
            var rlWorldState = worldState as BarbarianWorldState;
            rlWorldState.health = agent.GetComponent<BattleUnit>().health;

            var buildings = AIBattleManager.Instance.buildings;
            BattleBuilding archerTower = null;
            BattleBuilding townhall = null;
            float minDistanceArcherTower = float.MaxValue;
            float minDistanceTownhall = float.MaxValue;
            foreach (var building in buildings)
            {
                if (building.health > 0 && building.type == EBuildingType.archertower)
                {
                    var dis = Vector3.Distance(
                        agent.transform.position,
                        building.transform.position
                    );
                    if (dis < minDistanceArcherTower)
                    {
                        minDistanceArcherTower = dis;
                        archerTower = building;
                    }
                }
                if (building.health > 0 && building.type == EBuildingType.townhall)
                {
                    var dis = Vector3.Distance(
                        agent.transform.position,
                        building.transform.position
                    );
                    if (dis < minDistanceTownhall)
                    {
                        minDistanceTownhall = dis;
                        townhall = building;
                    }
                }
            }

            if (archerTower != null)
            {
                rlWorldState.hpDefense = archerTower.health;
                rlWorldState.distanceToDefense = minDistanceArcherTower;
                rlWorldState.targetDefense = archerTower.gameObject;
            }
            if (townhall != null)
            {
                rlWorldState.hpTownhall = townhall.health;
                rlWorldState.distanceToTownhall = minDistanceTownhall;
                rlWorldState.targetTownhall = townhall.gameObject;
            }

            rlWorldState.countUnit = AIBattleManager.Instance.units.Count;
        }
    }
}
