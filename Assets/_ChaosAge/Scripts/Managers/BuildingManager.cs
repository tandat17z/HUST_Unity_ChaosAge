using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.Data;
using ChaosAge.editor;
using ChaosAge.input;
using DatSystem;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        protected override void OnAwake()
        {

        }

        [Header("")]
        [SerializeField] BuildGrid grid;

        public BuildGrid Grid { get { return grid; } }

        public List<Building> Buildings { get => _buildings; }
        public Building SelectedBuilding => _selectedBuilding;

        private List<Building> _buildings = new();
        private Building _selectedBuilding;

        private Vector3 _buildingBasePosition;


        public void LoadMap(List<BuildingData> listBuildingData)
        {
            Clear();
            foreach (var data in listBuildingData)
            {
                var spawned = FactoryManager.Instance.SpawnBuilding(data.type);
                spawned.SetInfo(data.id, data.level);
                spawned.PlacedOnGrid(data.x, data.y);
                spawned.SetSelected(false);

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
            if (_selectedBuilding != building)
            {
                Unselect();

                _selectedBuilding = building;
                building.SetSelected(true);
                PanelManager.Instance.OpenPanel<UIBuildingInfo>();
            }
        }

        public void Unselect()
        {
            if (_selectedBuilding)
            {
                // bỏ chọn
                // cập nhật vị trí mới
                //
                _selectedBuilding.SetSelected(false);
                _selectedBuilding.ConfirmMove();

                var _playerData = DataManager.Instance.PlayerData;
                _playerData.UpdateBuildingData(_selectedBuilding.GetData());

                PanelManager.Instance.ClosePanel<UIBuildingInfo>();
            }
            _selectedBuilding = null;

        }
        public void Create(Building building)
        {
            // lưu vào data
            var data = new BuildingData(building.Type, building.CurrentX, building.CurrentY);
            DataManager.Instance.PlayerData.AddBuiling(data);

            building.SetInfo(data.id, data.level);
            _buildings.Add(building);


            Unselect();
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
