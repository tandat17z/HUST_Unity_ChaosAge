using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.Config;
using ChaosAge.Data;
using DatSystem;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        protected override void OnAwake() { }

        [SerializeField]
        private Grid grid;
        public Grid Grid
        {
            get => grid;
        }

        private PlayerData _playerData;

        public Building TownHall => _townhall;
        private Building _townhall;
        public Building SelectedBuilding => _selectedBuilding;
        private Building _selectedBuilding;
        private List<Building> _buildings = new List<Building>();

        #region Load map
        public void LoadMap(List<BuildingData> listBuildingData)
        {
            Clear();
            foreach (var data in listBuildingData)
            {
                var building = CreateBuilding(data);

                if (data.type == EBuildingType.TownHall)
                {
                    _townhall = building;
                }
            }
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

        #region Building
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
                $"{buildingType}_{level}"
            );
            if (buildingConfigSO == null)
            {
                return false;
            }
            foreach (var cost in buildingConfigSO.costs)
            {
                if (playerData.GetResource(cost.resourceType) < cost.quantity)
                {
                    return false;
                }
            }

            // Kiểm tra unlock level
            var townhallConfig = SOManager.Instance.GetSO<TownhallConfigSO>(
                $"{EBuildingType.TownHall}_{_townhall.Level}"
            );
            if (_townhall.Level < buildingConfigSO.unlockedLevel)
            {
                return false;
            }

            // Kiểm tra số lượng building
            if (
                GetBuildingNumber(buildingType)
                >= townhallConfig.GetLimitBuildingNumber(buildingType)
            )
            {
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
                    return true;
                }
            }
            return false;
        }

        public void CreateBuilding(EBuildingType buildingType)
        {
            var data = DataManager.Instance.CreateBuilding(
                buildingType,
                new Vector2(20, 20),
                false
            );

            if (_selectedBuilding != null)
            {
                _selectedBuilding.Deselect();
            }

            _selectedBuilding = CreateBuilding(data);
            _selectedBuilding.IsBuilding = true;
            _selectedBuilding.MoveTo(new Vector2(20, 20));

            _selectedBuilding.BuildingVisual.ShowBuildUI();
        }

        public Building CreateBuilding(BuildingData data)
        {
            var spawned = FactoryManager.Instance.SpawnBuilding(data.type);
            spawned.SetInfo(data);

            _buildings.Add(spawned);
            return spawned;
        }

        public void BeginBuild(Building building)
        {
            BeginUpgrade(building);
            DataManager.Instance.PlayerData.AddBuilding(building.GetData());
        }

        public void BeginUpgrade(Building building)
        {
            var playerData = DataManager.Instance.PlayerData;
            var buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>(
                $"{building.Type}_{building.Level + 1}"
            );
            foreach (var cost in buildingConfigSO.costs)
            {
                playerData.ReduceResource(cost.resourceType, cost.quantity);
            }
        }
        #endregion
    }
}
