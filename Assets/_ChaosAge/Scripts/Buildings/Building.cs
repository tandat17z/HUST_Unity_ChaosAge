namespace ChaosAge.building
{
    using System;
    using ChaosAge.Config;
    using ChaosAge.Data;
    using ChaosAge.manager;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class Building : MonoBehaviour
    {
        [SerializeField] EBuildingType type;
        [SerializeField] int rows = 1;
        [SerializeField] int columns = 1;
        [SerializeField] MeshRenderer baseArea;


        [SerializeField, ReadOnly] private string _id;
        [SerializeField, ReadOnly] private int _level;
        [SerializeField, ReadOnly] private int _x = 0; // tọa độ cũ trước khi move
        [SerializeField, ReadOnly] private int _y = 0;
        [SerializeField, ReadOnly] private int _currentX; // tọa độ hiện tại
        [SerializeField, ReadOnly] private int _currentY;

        public EBuildingType Type { get => type; }
        public int Level { get => _level; }
        public int CurrentX { get => _currentX; }
        public int CurrentY { get => _currentY; }
        public int Rows { get => rows; }
        public int Columns { get => columns; }




        public void SetInfo(string id, int level)
        {
            _id = id;
            _level = level;
        }

        public BuildingData GetData()
        {
            return new BuildingData()
            {
                id = _id,
                type = type,
                level = _level,

                x = _x,
                y = _y
            };
        }
        public void SetSelected(bool v)
        {
            baseArea.material.color = v ? Color.green : Color.white;
        }




        public void PlacedOnGrid(int x, int y)
        {
            _currentX = x;
            _currentY = y;

            _x = x;
            _y = y;

            Vector3 position = BuildingManager.Instance.Grid.GetCenterPosition(x, y, rows, columns);
            transform.position = position;
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

        public void ConfirmMove()
        {
            if (BuildingManager.Instance.CanPlaceBuilding(this))
            {
                _x = _currentX;
                _y = _currentY;
            }
            else
            {
                _currentX = _x;
                _currentY = _y;

                Vector3 position = BuildingManager.Instance.Grid.GetCenterPosition(_currentX, _currentY, rows, columns);
                transform.position = position;
            }
        }
    }

}
