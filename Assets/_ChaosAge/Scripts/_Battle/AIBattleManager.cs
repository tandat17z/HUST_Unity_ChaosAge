using System;
using System.Collections.Generic;
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
        public List<BattleBuilding> buildings;

        protected override void OnAwake() { }

        public void LoadLevel(int level)
        {
            BuildingManager.Instance.Clear();

            var opponentData = PlayerData.LoadFromFile($"Assets/Levels/{level}.json");
            Initialize(opponentData.buildings);
        }

        public void Initialize(List<BuildingData> buildings)
        {
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
        }

        public void DropUnit()
        {
            Debug.Log("DropUnit");
            var type = PanelManager.Instance.GetPanel<PanelBattle>().GetCurrentBuildingType();
            var pos = InputHandler.Instance.GetPointerPositionInMap();
            var posCell = BuildingManager.Instance.Grid.ConvertGridPos(pos);
            AddUnit(type, (int)posCell.x, (int)posCell.y);
        }

        public void AddUnit(EUnitType unitType, int x, int y) // ok
        {
            Debug.Log("AddUnit " + x + " " + y);
            var battleUnit = FactoryManager.Instance.SpawnUnit(EUnitType.AIAgent);
            var position = BattleVector2.GridToWorldPosition(new BattleVector2Int(x, y));
            var pos = BuildingManager.Instance.Grid.transform.TransformPoint(
                new Vector3(position.x, 0, position.y)
            );
            Debug.Log("AddUnit " + pos);
            battleUnit.transform.position = pos;
        }
    }
}
