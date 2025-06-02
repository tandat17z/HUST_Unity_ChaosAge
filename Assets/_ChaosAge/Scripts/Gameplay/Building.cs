namespace ChaosAge
{
    using System;
    using ChaosAge.Config;
    using ChaosAge.Data;
    using ChaosAge.manager;
    using DatSystem;
    using UnityEngine;

    public class Building : MonoBehaviour
    {
        [Header("Building Properties")]
        public string buildingName;
        public EBuildingType Type;
        public Vector2 gridPosition;

        [Header("Building Stats")]
        [SerializeField]
        private int id;

        [SerializeField]
        private int level = 1;

        [SerializeField]
        private int maxLevel = 3;

        [SerializeField]
        private int health = 100;

        [SerializeField]
        private int buildCost = 100;

        [SerializeField]
        private int upgradeCost = 50;

        [Header("Building Size")]
        public Vector2 size = new Vector2(3, 3); // Size in grid cells

        public BuildingVisual BuildingVisual => _buildingVisual;
        private BuildingVisual _buildingVisual;
        public int Id => id;
        public int Level => level;
        public int MaxLevel => maxLevel;
        public int Health => health;
        public int BuildCost => buildCost;
        public int UpgradeCost => upgradeCost;

        // For moving building
        private Vector2 originalGridPosition;
        private Vector2 offset;

        // For building
        public bool IsBuilding { get; set; } = false;

        private void Awake()
        {
            _buildingVisual = GetComponent<BuildingVisual>();
        }

        public void SetInfo(BuildingData buildingData)
        {
            this.id = buildingData.id;
            this.level = buildingData.level;
            this.Type = buildingData.type;
            this.gridPosition = new Vector2(buildingData.x, buildingData.y);

            SetGridPosition(gridPosition);
            _buildingVisual.SetVisual(level);
        }

        public void Select()
        {
            _buildingVisual?.OnBuildingSelected();
            _buildingVisual.ShowInfoUI();
        }

        public void Deselect()
        {
            _buildingVisual?.OnBuildingDeselected();
            _buildingVisual.HideInfoUI();
        }

        public void OverlapBuilding()
        {
            _buildingVisual?.OnBuildingOverlap();
        }

        public void StartMoving(Vector2 startCellPos)
        {
            originalGridPosition = gridPosition;
            offset = startCellPos - gridPosition;
        }

        public void MoveTo(Vector2 cellPos)
        {
            SetGridPosition(cellPos - offset);

            if (BuildingManager.Instance.CheckOverlapBuilding(this))
            {
                OverlapBuilding();
            }
            else
            {
                Select();
            }
        }

        public void StopMoving()
        {
            // TODO: Implement stopping movement
            if (BuildingManager.Instance.CheckOverlapBuilding(this) == true)
            {
                SetGridPosition(originalGridPosition);
            }
            Deselect();
            DataManager.Instance.SaveBuilding(this);

            GameManager.Instance.Log("Stop moving");
        }

        public void SetGridPosition(Vector2 newPosition)
        {
            gridPosition = newPosition;
            transform.position = BuildingManager.Instance.Grid.GetCenterPosition(
                (int)newPosition.x,
                (int)newPosition.y,
                (int)size.x,
                (int)size.y
            );
        }

        public void Upgrade()
        {
            if (level < maxLevel)
            {
                level++;
                DataManager.Instance.SaveBuilding(this);
                _buildingVisual.SetVisual(level);
            }
            else
            {
                GameManager.Instance.Log("Failed to upgrade, max level");
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                DestroyBuilding();
            }
        }

        private void DestroyBuilding()
        {
            // TODO: Add destruction effects
            Destroy(gameObject);
        }

        public bool IsCellPositionInBuilding(Vector2 cellPos)
        {
            return cellPos.x >= gridPosition.x
                && cellPos.x < gridPosition.x + size.x
                && cellPos.y >= gridPosition.y
                && cellPos.y < gridPosition.y + size.y;
        }

        public BuildingData GetData()
        {
            return new BuildingData(id, Type, level, gridPosition);
        }

        public void OnBuildOk()
        {
            if (BuildingManager.Instance.CheckOverlapBuilding(this))
            {
                GameManager.Instance.Log("Overlap building");
            }
            else
            {
                GameManager.Instance.Log("Build building");
                StopMoving();
                IsBuilding = false;
                _buildingVisual.HideBuildUI();

                // save building data
                var newBuildingData = GetData();
                newBuildingData.Save();
                DataManager.Instance.PlayerData.AddBuilding(newBuildingData);
            }
        }

        public void OnBuildCancel()
        {
            BuildingManager.Instance.DeselectBuilding();
            Destroy(gameObject);
        }
    }
}
