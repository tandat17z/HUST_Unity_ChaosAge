using System.Collections.Generic;
using AILibraryForNPC.Core;
using ChaosAge.Battle;
using ChaosAge.Config;
using ChaosAge.Data;
using ChaosAge.input;
using ChaosAge.manager;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.AI.battle
{
    public class AIBattleManager : Singleton<AIBattleManager>
    {
        [SerializeField]
        public bool StartBattle = false;

        [SerializeField]
        public List<BattleBuilding> buildings;
        public List<BattleUnit> units;

        protected override void OnAwake() { }

        public void LoadLevel(int level)
        {
            BuildingManager.Instance.Clear();

            var opponentData = PlayerData.LoadFromFile($"Assets/Levels/{level}.json");
            Initialize(opponentData.buildings);
        }

        public void Initialize(List<BuildingData> buildings)
        {
            this.units = new List<BattleUnit>();
            this.buildings = new List<BattleBuilding>();
            foreach (var data in buildings)
            {
                var b = FactoryManager.Instance.SpawnBuilding(data.type);
                b.SetInfo(data.id, data.level);
                b.PlacedOnGrid(data.x, data.y);

                var battleBuilding = b.GetComponent<BattleBuilding>();
                battleBuilding.SetInfo(data);
                this.buildings.Add(battleBuilding);
            }

            BuildingManager.Instance.UpdateNavMesh();

            StartBattle = true;
        }

        public void DropUnit()
        {
            var type = PanelManager.Instance.GetPanel<PanelBattle>().GetCurrentBuildingType();
            var pos = InputHandler.Instance.GetPointerPositionInMap();
            var posCell = BuildingManager.Instance.Grid.ConvertGridPos(pos);
            AddUnit(type, (int)posCell.x, (int)posCell.y);
        }

        public void AddUnit(EUnitType unitType, int x, int y) // ok
        {
            unitType = EUnitType.GOAPBarbarian;
            var battleUnit = FactoryManager.Instance.SpawnUnit(unitType);
            battleUnit.SetInfo();
            var position = BattleVector2.GridToWorldPosition(new BattleVector2Int(x, y));
            var pos = BuildingManager.Instance.Grid.transform.TransformPoint(
                new Vector3(position.x, 0, position.y)
            );

            battleUnit.transform.position = pos;

            battleUnit.GetComponent<BaseAgent>().IsStart = true;
            units.Add(battleUnit);
        }

        public void CreateBattle()
        {
            foreach (var building in buildings)
            {
                Destroy(building.gameObject);
            }
            buildings.Clear();

            var b = FactoryManager.Instance.SpawnBuilding(EBuildingType.townhall);
            var x = Random.Range(5, 35);
            var y = Random.Range(5, 35);
            b.SetInfo(0, 1);
            b.PlacedOnGrid(x, y);

            // archertower0
            var battleBuilding = b.GetComponent<BattleBuilding>();
            battleBuilding.SetInfo(new BuildingData(EBuildingType.townhall, x, y));
            buildings.Add(battleBuilding);

            b = FactoryManager.Instance.SpawnBuilding(EBuildingType.archertower);
            x = Random.Range(5, 20);
            y = Random.Range(5, 20);
            b.SetInfo(1, 1);
            b.PlacedOnGrid(x, y);

            battleBuilding = b.GetComponent<BattleBuilding>();
            battleBuilding.SetInfo(new BuildingData(EBuildingType.archertower, x, y));
            buildings.Add(battleBuilding);

            // archertower0
            b = FactoryManager.Instance.SpawnBuilding(EBuildingType.archertower);
            x = Random.Range(20, 35);
            y = Random.Range(5, 20);
            b.SetInfo(2, 1);
            b.PlacedOnGrid(x, y);

            battleBuilding = b.GetComponent<BattleBuilding>();
            battleBuilding.SetInfo(new BuildingData(EBuildingType.archertower, x, y));
            buildings.Add(battleBuilding);

            // archertower1
            b = FactoryManager.Instance.SpawnBuilding(EBuildingType.archertower);
            x = Random.Range(20, 35);
            y = Random.Range(20, 35);
            b.SetInfo(3, 1);
            b.PlacedOnGrid(x, y);

            battleBuilding = b.GetComponent<BattleBuilding>();
            battleBuilding.SetInfo(new BuildingData(EBuildingType.archertower, x, y));
            buildings.Add(battleBuilding);

            b = FactoryManager.Instance.SpawnBuilding(EBuildingType.archertower);
            x = Random.Range(5, 20);
            y = Random.Range(20, 35);
            b.SetInfo(4, 1);
            b.PlacedOnGrid(x, y);

            battleBuilding = b.GetComponent<BattleBuilding>();
            battleBuilding.SetInfo(new BuildingData(EBuildingType.archertower, x, y));
            buildings.Add(battleBuilding);

            BuildingManager.Instance.UpdateNavMesh();
        }

        void Update()
        {
            if (StartBattle)
            {
                if (buildings.Count == 0 || CheckTownHall() == false)
                {
                    CreateBattle();
                    return;
                }

                if (units.Count == 0)
                {
                    var rand = Random.Range(1, 10);
                    for (int i = 0; i < rand; i++)
                    {
                        var x = Random.Range(1, 39);
                        var y = Random.Range(1, 39);
                        AddUnit(EUnitType.GOAPBarbarian, x, y);
                    }
                }
            }
        }

        private bool CheckTownHall()
        {
            foreach (var building in buildings)
            {
                if (building.type == EBuildingType.townhall)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
