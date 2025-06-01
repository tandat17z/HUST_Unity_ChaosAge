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
        protected override void OnAwake()
        {
            buildings = new List<Building>();
        }

        [SerializeField]
        private Grid grid;
        public Grid Grid
        {
            get => grid;
        }

        [SerializeField]
        private Building selectedBuilding;
        private List<Building> buildings;
        private Building buildingToPlace;

        #region Load map
        public void LoadMap(List<BuildingData> listBuildingData)
        {
            Clear();
            int id = 0;
            foreach (var data in listBuildingData)
            {
                var spawned = FactoryManager.Instance.SpawnBuilding(data.type);
                spawned.SetInfo(id, data.level);
                spawned.PlacedOnGrid(data.x, data.y);
                id++;
            }
        }

        public void Clear()
        {
            foreach (var building in buildings)
            {
                Destroy(building.gameObject);
            }
            buildings.Clear();
        }

        #endregion

        #region Building
        public void SetBuildingToPlace(Building building)
        {
            if (selectedBuilding != null)
            {
                selectedBuilding.Deselect();
            }
            buildingToPlace = building;
        }

        public Building SelectBuilding(Vector2 gridPosition)
        {
            // Deselect current building if any
            if (selectedBuilding != null)
            {
                selectedBuilding.Deselect();
            }

            // Find building at grid position
            foreach (Building building in buildings)
            {
                if (IsPositionInBuilding(gridPosition, building))
                {
                    selectedBuilding = building;
                    selectedBuilding.Select();
                    return selectedBuilding;
                }
            }

            selectedBuilding = null;
            return null;
        }

        private bool IsPositionInBuilding(Vector2 position, Building building)
        {
            Vector2 buildingPos = building.gridPosition;
            Vector2 buildingSize = building.size;

            return position.x >= buildingPos.x
                && position.x < buildingPos.x + buildingSize.x
                && position.y >= buildingPos.y
                && position.y < buildingPos.y + buildingSize.y;
        }

        public bool CanPlaceBuilding(Building building, Vector2 gridPosition)
        {
            if (building == null)
                return false;

            // Check if position is within grid bounds
            if (!IsWithinGridBounds(gridPosition, building.size))
                return false;

            // Check for overlapping with other buildings
            foreach (Building existingBuilding in buildings)
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
            buildings.Add(building);
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
            if (building == selectedBuilding)
            {
                selectedBuilding = null;
            }
            buildings.Remove(building);
        }

#if UNITY_EDITOR
        [Button("Test")]
        public void Test()
        {
            SelectBuilding(new Vector2(0, 0));
        }
#endif
    }
        #endregion
}
