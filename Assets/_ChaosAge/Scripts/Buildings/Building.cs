using System;
using ChaosAge.Config;
using ChaosAge.Data;
using ChaosAge.manager;
using UnityEngine;

namespace ChaosAge.building
{
    public class Building : MonoBehaviour
    {
        [Serializable]
        public class Level
        {
            public int level = 1;
            public Sprite icon = null;
            public GameObject mesh = null;
        }

        [SerializeField] private int rows = 1;
        [SerializeField] private int columns = 1;

        [SerializeField] private MeshRenderer baseArea = null;
        [SerializeField] private Level[] levels;

        public int CurrentX { get => _currentX; }
        public int CurrentY { get => _currentY; }
        public int Rows { get => rows; }
        public int Columns { get => columns; }



        private int _currentX = 0;
        private int _currentY = 0;
        private int _x = 0;
        private int _y = 0;

        private string _id;
        private EBuildingType _type;
        private int _level;

        // Đặt trên g
        public void PlacedOnGrid(int x, int y)
        {
            _currentX = x;
            _currentY = y;

            _x = x;
            _y = y;

            Vector3 position = BuildingManager.Instance.Grid.GetCenterPosition(x, y, rows, columns);
            transform.position = position;

            SetBaseColor();
        }

        public void StartMovingOnGrid()
        {
            _x = _currentX;
            _y = _currentY;
        }

        public void RemovedFromGrid()
        {
            Destroy(gameObject);
        }

        public void UpdateGridPosition(Vector3 basePosition, Vector3 currentPosition)
        {
            var grid = BuildingManager.Instance.Grid;
            Vector3 dir = grid.transform.InverseTransformPoint(currentPosition) - grid.transform.InverseTransformPoint(basePosition);
            int xDis = Mathf.RoundToInt(dir.x / grid.CellSize);
            int yDis = Mathf.RoundToInt(dir.z / grid.CellSize);

            _currentX = _x + xDis;
            _currentY = _y + yDis;

            Vector3 position = BuildingManager.Instance.Grid.GetCenterPosition(_currentX, _currentY, rows, columns);
            transform.position = position;
            SetBaseColor();
        }

        public void SetBaseColor()
        {
            if (BuildingManager.Instance.CanPlaceBuilding(this))
            {
                baseArea.material.color = Color.green;
            }
            else
            {
                baseArea.material.color = Color.red;
            }
        }

        public void SetSelected(bool v)
        {
            baseArea.material.color = v ? Color.green : Color.white;
        }

        public BuildingData GetData()
        {
            return new BuildingData()
            {
                id = _id,
                type = _type,
                level = _level
            };
        }
    }

}
