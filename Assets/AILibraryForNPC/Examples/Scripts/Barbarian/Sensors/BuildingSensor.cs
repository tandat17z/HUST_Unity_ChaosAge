using System;
using AILibraryForNPC.Core;
using ChaosAge.AI.battle;
using ChaosAge.Config;
using UnityEngine;

namespace AILibraryForNPC.Examples
{
    public class BuildingSensor : BaseSensor_v2
    {
        private float _hpDefense;
        private float _distanceToDefense;
        private GameObject _targetDefense;

        private float _hpTownhall;
        private float _distanceToTownhall;
        private GameObject _targetTownhall;

        public override void Observe(WorldState_v2 worldstate)
        {
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
                _hpDefense = archerTower.health;
                _distanceToDefense = minDistanceArcherTower;
                _targetDefense = archerTower.gameObject;
                SetDefenseInfo(worldstate);
            }
            if (townhall != null)
            {
                _hpTownhall = townhall.health;
                _distanceToTownhall = minDistanceTownhall;
                _targetTownhall = townhall.gameObject;
                SetTownhallInfo(worldstate);
            }
        }

        private void SetDefenseInfo(WorldState_v2 worldstate)
        {
            worldstate.AddState("hpDefense", _hpDefense);
            worldstate.AddState("distanceToDefense", _distanceToDefense);
            worldstate.AddState("targetDefenseX", _targetDefense.transform.position.x);
            worldstate.AddState("targetDefenseY", _targetDefense.transform.position.y);

            worldstate.AddBuffer("targetDefense", _targetDefense);
        }

        private void SetTownhallInfo(WorldState_v2 worldstate)
        {
            worldstate.AddState("hpTownhall", _hpTownhall);
            worldstate.AddState("distanceToTownhall", _distanceToTownhall);
            worldstate.AddState("targetTownhallX", _targetTownhall.transform.position.x);
            worldstate.AddState("targetTownhallY", _targetTownhall.transform.position.y);

            worldstate.AddBuffer("targetTownhall", _targetTownhall);
        }
    }
}
