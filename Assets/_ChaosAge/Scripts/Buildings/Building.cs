namespace ChaosAge.building
{
    using ChaosAge.Config;
    using ChaosAge.Data;
    using ChaosAge.manager;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class Building0 : MonoBehaviour
    {
        [SerializeField]
        EBuildingType type;

        [SerializeField]
        MeshRenderer baseArea;

        [SerializeField]
        MeshRenderer layoutLevel;

        [SerializeField, ReadOnly]
        private int _id;

        [SerializeField, ReadOnly]
        private int _level;

        [SerializeField, ReadOnly]
        int _rows;

        [SerializeField, ReadOnly]
        int _columns;

        [SerializeField, ReadOnly]
        private int _x = 0; // tọa độ cũ trước khi move

        [SerializeField, ReadOnly]
        private int _y = 0;

        [SerializeField, ReadOnly]
        private int _currentX; // tọa độ hiện tại

        [SerializeField, ReadOnly]
        private int _currentY;

        public EBuildingType Type
        {
            get => type;
        }
        public int Level
        {
            get => _level;
        }
        public int CurrentX
        {
            get => _currentX;
        }
        public int CurrentY
        {
            get => _currentY;
        }
        public int Rows
        {
            get => _rows;
        }
        public int Columns
        {
            get => _rows;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            (_rows, _columns) = GameConfig
                .LoadFromFile("Assets/_ChaosAge/Config.json")
                .GetBuildingSize(type);
            baseArea.transform.localScale = new Vector3(_rows, _columns, 1);
        }
#endif

        public void SetInfo(int id, int level)
        {
            _id = id;
            _level = level;

            Visual();
        }

        public BuildingData GetData()
        {
            return null;
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

            Vector3 position = BuildingManager.Instance.Grid.GetCenterPosition(
                x,
                y,
                _rows,
                _columns
            );
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
            Vector3 dir =
                grid.transform.InverseTransformPoint(currentPosition)
                - grid.transform.InverseTransformPoint(basePosition);
            int xDis = Mathf.RoundToInt(dir.x / grid.CellSize);
            int yDis = Mathf.RoundToInt(dir.z / grid.CellSize);

            _currentX = _x + xDis;
            _currentY = _y + yDis;

            Vector3 position = BuildingManager.Instance.Grid.GetCenterPosition(
                _currentX,
                _currentY,
                _rows,
                _columns
            );
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

                Vector3 position = BuildingManager.Instance.Grid.GetCenterPosition(
                    _currentX,
                    _currentY,
                    _rows,
                    _columns
                );
                transform.position = position;
            }
        }

        public void Upgrade()
        {
            _level += 1;

            Visual();
        }

        public void Visual()
        {
            Color[] colors = { Color.white, Color.gray, Color.blue, Color.green, Color.cyan };
            layoutLevel.material.color = colors[_level];
        }
    }
}
