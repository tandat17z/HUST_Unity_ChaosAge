using System;
using System.Collections.Generic;
using System.IO;
using AILibraryForNPC.Core;
using ChaosAge.Battle;
using ChaosAge.building;
using ChaosAge.data;
using ChaosAge.manager;
using ChaosAge.spawner;
using DatSystem;
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
        private List<BattleUnit> _units;
        public List<BattleUnit> units => _units;

        private List<List<bool>> _canMoveCells;

        public GameObject home;

        public Action<BattleBuilding> OnRemoveBuilding;
        public Action<BattleUnit> OnRemoveUnit;

        public int CurrentLevel { get { return PlayerPrefs.GetInt("Battle_CurrentLevel", 1); } set { PlayerPrefs.SetInt("Battle_CurrentLevel", value); } }

        protected override void OnAwake()
        {
            OnRemoveBuilding += RemoveTownHall;
            OnRemoveUnit += CheckLose;

            _canMoveCells = new List<List<bool>>();
            for (int i = 0; i < MaxCell; i++)
            {
                _canMoveCells.Add(new List<bool>());
                for (int j = 0; j < MaxCell; j++)
                {
                    _canMoveCells[i].Add(true);
                }
            }
        }

        private void CheckLose(BattleUnit unit)
        {
            var playerData = DataManager.Instance.PlayerData;
            if (units.Count == 0 && playerData.units.Count == 0)
            {
                SetResult(EGameState.Lose);
            }
        }

        private void RemoveTownHall(BattleBuilding building)
        {
            Debug.Log("Check event RemoveTownHall");
            if (building.Type == EBuildingType.TownHall)
            {
                SetResult(EGameState.Win, 1);
            }
        }

        void OnDestroy()
        {
            OnRemoveBuilding -= RemoveTownHall;
            OnRemoveUnit -= CheckLose;
        }

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
            Reset();

            _units = new List<BattleUnit>();
            _buildings = new List<BattleBuilding>();

            LoadLevel(level);

            foreach (var building in BuildingManager.Instance.Buildings)
            {
                var battleBuilding = building.gameObject.AddComponent<BattleBuilding>();
                battleBuilding.Init();
                _buildings.Add(battleBuilding);
            }
            ActiveBuildingAgent();
            InitCanMoveCells();

            home = new GameObject("Home");
            home.transform.position = GetWorldPosition(new Vector2(5, 5));
            UpdateNavMesh();
        }

        private void InitCanMoveCells()
        {
            ResetCanMoveCells();

            foreach (var battleBuilding in _buildings)
            {
                var building = battleBuilding.GetComponent<Building>();
                var xCell = (int)building.gridPosition.x;
                var yCell = (int)building.gridPosition.y;
                var cellSize = building.size;
                for (int i = -1; i <= cellSize.x; i++)
                {
                    for (int j = -1; j <= cellSize.y; j++)
                    {
                        var xIdx = xCell + i;
                        var yIdx = yCell + j;
                        if (xIdx >= 0 && xIdx < MaxCell && yIdx >= 0 && yIdx < MaxCell)
                        {
                            _canMoveCells[xIdx][yIdx] = false;
                        }
                    }
                }
            }
        }

        private void ResetCanMoveCells()
        {
            for (int i = 0; i < MaxCell; i++)
            {
                for (int j = 0; j < MaxCell; j++)
                {
                    _canMoveCells[i][j] = true;
                }
            }
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
            _units.Add(battleUnit);

            var playerData = DataManager.Instance.PlayerData;
            playerData.ReduceUnit(unitType, 1);
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
            if (cell.x < 0 || cell.x >= MaxCell || cell.y < 0 || cell.y >= MaxCell)
            {
                return false;
            }
            return _canMoveCells[(int)cell.x][(int)cell.y];
        }

        public void UpdateNavMesh()
        {
            BuildingManager.Instance.UpdateNavMesh();
        }

        public void SetResult(EGameState state, int star = 0)
        {
            Reset();
            if (state == EGameState.Win)
            {
                Debug.Log("Win");
                PanelManager.Instance.ClosePanel<PanelBattle>();
                PanelManager.Instance.OpenPanel<PopupWin>();
            }
            else
            {
                // khi het thoi gian
                // khi het quân còn sống
                // khi tự kết thúc
                Debug.Log("Lose");
                PanelManager.Instance.OpenPanel<PopupLoss>();
            }
        }

        public void RemoveBuilding(BattleBuilding building)
        {
            buildings.Remove(building);
            BuildingManager.Instance.Buildings.Remove(building.GetComponent<Building>());
            Destroy(building.gameObject);

            OnRemoveBuilding?.Invoke(building);
        }

        public void RemoveUnit(BattleUnit battleUnit)
        {
            _units.Remove(battleUnit);
            Destroy(battleUnit.gameObject);
            OnRemoveUnit?.Invoke(battleUnit);
        }

        public void Reset()
        {
            if (_units == null) return;
            foreach (var unit in _units)
            {
                Destroy(unit.gameObject);
            }
            _units.Clear();
        }

        public bool CheckEnoughUnit(EUnitType unitType)
        {
            var playerData = DataManager.Instance.PlayerData;
            return playerData.GetUnitNum(unitType) > 0;
        }

        public void TryAddUnit(EUnitType unitType, Vector2 cellPos)
        {
            if (CheckEnoughUnit(unitType))
            {
                if (CheckCanMoveOnCell(cellPos))
                {
                    AddUnit(unitType, cellPos);
                }
                else
                {
                    GameManager.Instance.Log("Can not move to this cell");
                }
            }
            else
            {
                GameManager.Instance.Log("Not enough units");
            }
        }
    }

    public enum EGameState
    {
        Win,
        Lose
    }
}
