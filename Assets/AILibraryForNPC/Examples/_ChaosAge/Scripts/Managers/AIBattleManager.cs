using System;
using System.Collections.Generic;
using System.IO;
using AILibraryForNPC.Core;
using ChaosAge.Battle;
using ChaosAge.data;
using ChaosAge.manager;
using ChaosAge.spawner;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.AI.battle
{
    public class AIBattleManager : Singleton<AIBattleManager>
    {
        public const int MaxCell = 40;

        public List<BattleBuilding> buildings => _buildings;
        private List<BattleBuilding> _buildings;
        public List<BattleUnit> units => _units;
        private List<BattleUnit> _units;

        private bool[,] _canMoveCells;

        public GameObject home;

        protected override void OnAwake() { }

        public void LoadLevel(int level)
        {
            var filePath = $"Assets/Levels/{level}.json";
            string json = File.ReadAllText(filePath);
            var buildingFile = JsonUtility.FromJson<PlayerData.BuildingFile>(json);

            BuildingManager.Instance.LoadMap(buildingFile.listBuilding, true);
        }

        private void ActiveBuildingAgent()
        {
            foreach (var building in _buildings)
            {
                if (building.TryGetComponent<BaseAgent>(out var agent))
                {
                    agent.IsStart = true;
                }
            }
        }

        public void Initialize(int level)
        {
            LoadLevel(level);

            _units = new List<BattleUnit>();
            _buildings = new List<BattleBuilding>();
            foreach (var building in BuildingManager.Instance.Buildings)
            {
                var battleBuilding = building.gameObject.AddComponent<BattleBuilding>();
                battleBuilding.Init();
                _buildings.Add(battleBuilding);
            }
            ActiveBuildingAgent();

            home = new GameObject("Home");
            home.transform.position = GetWorldPosition(new Vector2(5, 5));
            UpdateNavMesh();
        }

        // public void DropUnit()
        // {
        //     var type = PanelManager.Instance.GetPanel<PanelBattle>().GetCurrentBuildingType();
        //     // var pos = InputHandler.Instance.GetPointerPositionInMap();
        //     var pos = Vector3.zero;
        //     // var posCell = BuildingManager.Instance.Grid.ConvertGridPos(pos);
        //     var posCell = new Vector2(0, 0);
        //     AddUnit(type, (int)posCell.x, (int)posCell.y);
        // }

        public void AddUnit(EUnitType unitType, Vector2 cell) // ok
        {
            var battleUnit = FactoryManager.Instance.SpawnUnit(unitType);
            battleUnit.Initialize(cell);

            battleUnit.GetComponent<BaseAgent>().IsStart = true;
            units.Add(battleUnit);
        }

        // public void CreateBattle()
        // {
        //     foreach (var building in buildings)
        //     {
        //         Destroy(building.gameObject);
        //     }
        //     buildings.Clear();

        //     ResetCanMoveCells();
        //     foreach (var building in buildings)
        //     {
        //         var xCell = building.battleBuidlingConfig.x;
        //         var yCell = building.battleBuidlingConfig.y;
        //         _canMoveCells[xCell, yCell] = false;
        //     }
        // }

        // private void ResetCanMoveCells()
        // {
        //     _canMoveCells = new bool[MaxCell, MaxCell];
        //     for (int i = 0; i < MaxCell; i++)
        //     {
        //         for (int j = 0; j < MaxCell; j++)
        //         {
        //             _canMoveCells[i, j] = true;
        //         }
        //     }
        // }

        // void Update()
        // {
        //     if (StartBattle)
        //     {
        //         if (buildings.Count == 0 || CheckTownHall() == false)
        //         {
        //             CreateBattle();
        //             ActiveBuildingAgent();
        //             return;
        //         }

        //         // if (units.Count == 0)
        //         // {
        //         //     var rand = Random.Range(1, 10);
        //         //     for (int i = 0; i < rand; i++)
        //         //     {
        //         //         var x = Random.Range(1, 39);
        //         //         var y = Random.Range(1, 39);
        //         //         AddUnit(EUnitType.GOAPBarbarian, x, y);
        //         //     }
        //         // }
        //     }
        // }

        // private bool CheckTownHall()
        // {
        //     foreach (var building in buildings)
        //     {
        //         if (building.type == EBuildingType.TownHall)
        //         {
        //             return true;
        //         }
        //     }
        //     return false;
        // }

        // public bool CanMove(float nextX, float nextY)
        // {
        //     // ô này trong phạm vi cho phép và không đè vào công trình nào
        //     return true;
        // }

        public Vector2 GetCell(Vector3 position)
        {
            // return BuildingManager.Instance.Grid.ConvertGridPos(position);
            return new Vector2(0, 0);
        }

        public Vector3 GetWorldPosition(Vector2 cell)
        {
            return BuildingManager.Instance.Grid.GetCenterPosition((int)cell.x, (int)cell.y, 1, 1);
        }

        public bool CheckCanMoveOnCell(Vector2 cell)
        {
            return _canMoveCells[(int)cell.x, (int)cell.y];
        }

        public void UpdateNavMesh()
        {
            BuildingManager.Instance.UpdateNavMesh();
        }

        public void SetResult(EGameState state)
        {
            if (state == EGameState.Win)
            {
                Debug.Log("Win");
                PanelManager.Instance.OpenPanel<PopupWin>();
            }
            else
            {
                Debug.Log("Lose");
                PanelManager.Instance.OpenPanel<PopupLoss>();
            }
        }
    }

    public enum EGameState
    {
        Win,
        Lose
    }
}
