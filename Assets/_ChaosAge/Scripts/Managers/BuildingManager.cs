using System;
using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.camera;
using ChaosAge.editor;
using ChaosAge.input;
using DatSystem;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;
using UnityEngine.Windows;

namespace ChaosAge.manager
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        protected override void OnAwake()
        {

        }



        [SerializeField] Building[] prefabs;

        [Header("")]
        [SerializeField] CameraController cameraController;
        [SerializeField] BuildGrid grid;

        public CameraController CameraController { get { return cameraController; } }
        public BuildGrid Grid { get { return grid; } }

        public Building SelectedBuilding => _selectedBuilding;

        private List<Building> _buildings = new();
        private Building _selectedBuilding;

        private Vector3 _buildingBasePosition;


        /// <summary>
        /// Kiểm tra có thể đặt building trên map không
        /// </summary>
        /// <param name="building"></param>
        /// <returns></returns>
        public bool CanPlaceBuilding(Building building)
        {
            if (building.CurrentX < 0 || building.CurrentY < 0
              || building.CurrentX + building.Columns >= grid.Column
              || building.CurrentY + building.Rows >= grid.Row)
            {
                return false;
            }

            Rect rectBuilding = new Rect(building.CurrentX, building.CurrentY, building.Columns, building.Rows);
            foreach (var b in _buildings)
            {
                if (b != building)
                {
                    Rect rect = new Rect(b.CurrentX, b.CurrentY, b.Columns, b.Rows);

                    if (rectBuilding.Overlaps(rect))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Building SelectAtPosition(Vector3 poinerPosInPlane)
        {
            foreach (Building building in _buildings)
            {
                if (grid.IsWorldPositionIsOnPlane(poinerPosInPlane, building))
                {
                    Select(building);
                    return building;
                }
            }
            return null;
        }

        public void Select(Building building)
        {
            Unselect();

            _selectedBuilding = building;
            building.SetSelected(true);
            PanelManager.Instance.OpenPanel<UIBuildingInfo>();
        }

        public void Unselect()
        {
            if (_selectedBuilding)
            {
                _selectedBuilding.SetSelected(false);

                PanelManager.Instance.ClosePanel<UIBuildingInfo>();
            }
            _selectedBuilding = null;

        }
        public void AddListBuilding(Building building)
        {
            // lưu vào data
            DataManager.Instance.PlayerData.AddBuiling(building.GetData());
            _buildings.Add(building);
        }

        public Building HasBuildingAtPosition(Vector3 posInPlane)
        {
            if (_selectedBuilding != null && grid.IsWorldPositionIsOnPlane(posInPlane, _selectedBuilding))
            {
                return _selectedBuilding;
            }

            foreach (Building building in _buildings)
            {
                if (grid.IsWorldPositionIsOnPlane(posInPlane, building))
                {
                    return building;
                }
            }
            return null;
        }

        public void StartMove(Vector3 poinerPosInPlane)
        {
            _selectedBuilding.StartMovingOnGrid();
            _buildingBasePosition = poinerPosInPlane;
        }

        private void Update()
        {
            if (InputHandler.Instance.MoveBuilding)
            {
                var currentPosition = InputHandler.Instance.GetPointerPositionInMap();

                _selectedBuilding.UpdateGridPosition(_buildingBasePosition, currentPosition);
            }
        }
    }

}
