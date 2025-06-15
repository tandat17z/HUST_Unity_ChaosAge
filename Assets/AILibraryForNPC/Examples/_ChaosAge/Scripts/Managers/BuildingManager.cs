namespace ChaosAge.manager
{
    using System;
    using System.Collections.Generic;
    using ChaosAge.building;
    using ChaosAge.data;
    using ChaosAge.map;
    using ChaosAge.spawner;
    using DatSystem;
    using DatSystem.UI;
    using DatSystem.utils;
    using DG.Tweening;
    using Unity.AI.Navigation;
    using UnityEngine;

    public class BuildingManager : Singleton<BuildingManager>
    {
        protected override void OnAwake() { }

        [SerializeField]
        private MapGrid grid;
        [SerializeField]
        private NavMeshSurface navMeshSurface;
        public MapGrid Grid
        {
            get => grid;
        }

        private PlayerData _playerData;

        public Building TownHall => _townhall;
        private Building _townhall;
        public Building SelectedBuilding => _selectedBuilding;
        private Building _selectedBuilding;

        public List<Building> Buildings => _buildings;
        private List<Building> _buildings = new List<Building>();

        public static Action OnCompleteUpgrade;

        #region Load map
        public void LoadMap(List<BuildingData> listBuildingData, bool isBattle = false)
        {
            Clear();
            foreach (var data in listBuildingData)
            {
                var building = CreateBuilding(data, isBattle);

                if (data.type == EBuildingType.TownHall)
                {
                    _townhall = building;
                }
            }

            DataManager.Instance.UpdateMaxResource();
        }

        public void Clear()
        {
            foreach (var building in _buildings)
            {
                Destroy(building.gameObject);
            }
            _buildings.Clear();
        }

        #endregion

        #region Select/Deselect
        public Building SelectBuilding(Vector2 gridPosition)
        {
            Debug.Log("SelectBuilding: " + gridPosition);
            // Deselect current building if any
            DeselectBuilding();

            // Find building at grid position
            foreach (Building building in _buildings)
            {
                if (building.IsCellPositionInBuilding(gridPosition))
                {
                    _selectedBuilding = building;
                    _selectedBuilding.Select();
                    return _selectedBuilding;
                }
            }

            _selectedBuilding = null;
            return null;
        }

        public void DeselectBuilding()
        {
            if (_selectedBuilding != null)
            {
                _selectedBuilding.Deselect();
                _selectedBuilding = null;
            }
        }
        #endregion

        #region Check
        private bool DoBuildingsOverlap(Vector2 position1, Vector2 size1, Building building2)
        {
            Vector2 position2 = building2.gridPosition;
            Vector2 size2 = building2.size;

            return !(
                position1.x + size1.x <= position2.x
                || position2.x + size2.x <= position1.x
                || position1.y + size1.y <= position2.y
                || position2.y + size2.y <= position1.y
            );
        }

        public bool CanUpgradeBuilding(EBuildingType buildingType, int level)
        {
            // Kiểm tra cost
            var playerData = DataManager.Instance.PlayerData;
            var buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>(
                $"{buildingType}_{level + 1}"
            );
            if (buildingConfigSO == null)
            {
                return false;
            }
            foreach (var cost in buildingConfigSO.costs)
            {
                if (playerData.GetResource(cost.resourceType) < cost.quantity)
                {
                    Debug.LogWarning(
                        $"Cannot upgrade building {buildingType} level {level} because player has not enough resource {cost.resourceType} {cost.quantity}"
                    );
                    return false;
                }
            }

            // Kiểm tra unlock level
            var townhallConfig = SOManager.Instance.GetSO<TownhallConfigSO>(
                $"{EBuildingType.TownHall}_{_townhall.Level}"
            );
            if (_townhall.Level < buildingConfigSO.unlockedLevel)
            {
                Debug.LogWarning(
                    $"Cannot upgrade building {buildingType} level {level} because townhall level is not enough {_townhall.Level} < {buildingConfigSO.unlockedLevel}"
                );
                return false;
            }

            // Kiểm tra số lượng building
            if (level == 0 &&
                GetBuildingNumber(buildingType)
                >= townhallConfig.GetLimitBuildingNumber(buildingType)
            )
            {
                Debug.LogWarning(
                    $"Cannot upgrade building {buildingType} level {level} because building number is not enough {GetBuildingNumber(buildingType)} >= {townhallConfig.GetLimitBuildingNumber(buildingType)}"
                );
                return false;
            }

            return true;
        }

        public int GetBuildingNumber(EBuildingType type)
        {
            return _buildings.FindAll(building => building.Type == type).Count;
        }

        public bool CanPlaceBuilding(Building selectedBuilding)
        {
            foreach (Building building in _buildings)
            {
                if (
                    building != selectedBuilding
                    && DoBuildingsOverlap(
                        selectedBuilding.gridPosition,
                        selectedBuilding.size,
                        building
                    )
                )
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Create
        public void CreateBuilding(EBuildingType buildingType, bool isBattle = false, int x = 20, int y = 20)
        {
            var data = DataManager.Instance.CreateBuilding(
                buildingType,
                0,
                new Vector2(x, y),
                false
            );

            if (_selectedBuilding != null)
            {
                _selectedBuilding.Deselect();
            }

            _selectedBuilding = CreateBuilding(data, isBattle);
            _selectedBuilding.Select();
            _selectedBuilding.MoveTo(new Vector2(x, y));

            _selectedBuilding.BuildingVisual.ShowBuildUI();
        }

        public Building CreateBuilding(BuildingData data, bool isBattle = false)
        {
            var spawned = FactoryManager.Instance.SpawnBuilding(data.type);
            spawned.SetInfo(data);
            if (isBattle)
            {
                spawned.StopRunning();
            }

            _buildings.Add(spawned);
            return spawned;
        }

        #endregion

        #region Build/Upgrade

        public void OnBuildOk(Building building)
        {
            if (CanPlaceBuilding(building) == false)
            {
                GameManager.Instance.Log("Overlap building");
            }
            else
            {
                StartBuild(building);
            }
        }

        public void StartBuild(Building building)
        {
            StartUpgrade(building);
            DataManager.Instance.PlayerData.AddBuilding(building.GetData());
        }

        public void StartUpgrade(Building building)
        {
            // Trừ tài nguyên
            var playerData = DataManager.Instance.PlayerData;
            var buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>(
                $"{building.Type}_{building.Level + 1}"
            );
            foreach (var cost in buildingConfigSO.costs)
            {
                playerData.ReduceResource(cost.resourceType, cost.quantity);
            }

            // Bắt đầu upgrade
            building.StartUpgrade();
        }

        public void CompleteUpgradeByTime(Building building)
        {
            building.CompleteUpgradeByTime();
        }

        public void OnBuildCancel(Building building)
        {
            _buildings.Remove(building);
            building.Deselect();
            Destroy(building.gameObject);
        }

        #endregion

        public void UpdateNavMesh(){
            DOVirtual.DelayedCall(0.25f, () => {
            // Xóa NavMesh cũ và tạo NavMesh mới
                navMeshSurface.RemoveData(); // Xóa NavMesh cũ
                navMeshSurface.BuildNavMesh(); // Tạo NavMesh mới
                Debug.Log("NavMesh đã được cập nhật!");
            });
        }
    }
}
