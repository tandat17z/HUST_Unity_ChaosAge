using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.Data;
using DatSystem.utils;
using Sirenix.OdinInspector;
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

        public Building SelectedBuilding => _selectedBuilding;
        private Building _selectedBuilding;
        private List<Building> _buildings = new List<Building>();

        #region Load map
        public void LoadMap(List<BuildingData> listBuildingData)
        {
            Clear();
            foreach (var data in listBuildingData)
            {
                var spawned = FactoryManager.Instance.SpawnBuilding(data.type);
                spawned.SetInfo(data);

                _buildings.Add(spawned);
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

        public bool CanPlaceBuilding(Building building, Vector2 gridPosition)
        {
            if (building == null)
                return false;

            // Check if position is within grid bounds
            if (!IsWithinGridBounds(gridPosition, building.size))
                return false;

            // Check for overlapping with other buildings
            foreach (Building existingBuilding in _buildings)
            {
                if (DoBuildingsOverlap(gridPosition, building.size, existingBuilding))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsWithinGridBounds(Vector2 position, Vector2 size)
        {
            // TODO: Implement grid bounds check based on your grid system
            return true;
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

        public void PlaceBuilding(Building building, Vector2 gridPosition)
        {
            if (!CanPlaceBuilding(building, gridPosition))
                return;

            building.SetGridPosition(gridPosition);
            _buildings.Add(building);
        }

        public void MoveBuilding(Building building, Vector2 newGridPosition)
        {
            if (!CanPlaceBuilding(building, newGridPosition))
                return;

            building.SetGridPosition(newGridPosition);
        }

        public bool CanUpgradeBuilding(Building building)
        {
            if (building == null)
                return false;
            return building.Level < building.MaxLevel;
        }

        public void UpgradeBuilding(Building building)
        {
            if (!CanUpgradeBuilding(building))
                return;

            building.Upgrade();
        }

        public void RemoveBuilding(Building building)
        {
            if (building == _selectedBuilding)
            {
                _selectedBuilding = null;
            }
            _buildings.Remove(building);
        }
        #endregion
    }
}
