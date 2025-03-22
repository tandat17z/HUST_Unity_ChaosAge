using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.camera;
using ChaosAge.editor;
using DatSystem;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] Building[] prefabs;

        [Header("")]
        [SerializeField] CameraController cameraController;
        [SerializeField] BuildGrid grid;

        public CameraController CameraController { get { return cameraController; } }
        public BuildGrid Grid { get { return grid; } }

        public Building[] Prefabs { get => prefabs; }
        public Building SelectedBuilding => _selectedBuilding;

        private List<Building> _buildings = new();
        private Building _selectedBuilding;



        public bool CanPlaceBuilding(Building building)
        {
            if (building.CurrentX < 0 || building.CurrentY < 0
              || building.CurrentX + building.Columns >= grid.Column
              || building.CurrentY + building.Rows >= grid.Row)
            {
                return false;
            }

            Rect rectBuilding = new Rect(building.CurrentX, building.CurrentY, building.Columns, building.Rows);
            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i] != building)
                {
                    Rect rect = new Rect(_buildings[i].CurrentX, _buildings[i].CurrentY, _buildings[i].Columns, _buildings[i].Rows);

                    if (rectBuilding.Overlaps(rect))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected override void OnAwake()
        {

        }

        public void SelectBuilding(Vector3 poinerPosInPlane)
        {
            foreach (Building building in _buildings)
            {
                if (grid.IsWorldPositionIsOnPlane(poinerPosInPlane, building))
                {
                    Debug.Log("SelectBuilding");
                    SelectBuilding(building);
                    return;
                }
            }
        }

        public void SelectBuilding(Building building)
        {
            if (_selectedBuilding) _selectedBuilding.SetSelected(false);

            _selectedBuilding = building;
            if (building) building.SetSelected(true);
        }

        public void AddListBuilding(Building building)
        {
            // l?u vào data
            DataManager.Instance.PlayerData.AddBuiling(building.GetData());
            _buildings.Add(building);

        }
    }

}
