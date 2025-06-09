using System;
using AILibraryForNPC.Core;
using ChaosAge.AI.battle;
using ChaosAge.data;
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
            _distanceToDefense = float.MaxValue;
            _distanceToTownhall = float.MaxValue;
            foreach (var building in buildings)
            {
                if (building.health > 0 && building.Type == EBuildingType.ArcherTowner)
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
                if (building.health > 0 && building.Type == EBuildingType.TownHall)
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

            if (_distanceToDefense < _distanceToTownhall)
            {
                worldstate.AddBuffer("target", _targetDefense.GetComponent<BattleBuilding>());
                worldstate.AddState("TargetIsDefense", 0);
            }
            else
            {
                worldstate.AddBuffer("target", _targetTownhall.GetComponent<BattleBuilding>());
                worldstate.AddState("TargetIsTownhall", 1);
            }
        }

        private void SetDefenseInfo(WorldState_v2 worldstate)
        {
            worldstate.AddState("hpDefense", GetHpLevel(_hpDefense));
            worldstate.AddState("distanceToDefense", GetDistanceLevel(_distanceToDefense));
            worldstate.AddBuffer("targetDefense", _targetDefense);
        }

        private void SetTownhallInfo(WorldState_v2 worldstate)
        {
            worldstate.AddState("hpTownhall", GetHpLevel(_hpTownhall));
            worldstate.AddState("distanceToTownhall", GetDistanceLevel(_distanceToTownhall));
            worldstate.AddBuffer("targetTownhall", _targetTownhall);
        }

        private int GetHpLevel(float hp)
        {
            if (hp <= 20)
                return 0;
            return 1;
        }

        private int GetDistanceLevel(float distance)
        {
            if (distance <= 3)
                return 0;
            if (distance <= 15)
                return 1;
            return 2;
        }
    }
}
