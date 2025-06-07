namespace ChaosAge.building
{
    using System;
    using ChaosAge.data;
    using ChaosAge.manager;
    using DatSystem;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.VFX;

    public class Building : MonoBehaviour
    {
        [Header("Building Properties")]
        public EBuildingType Type;

        [SerializeField, ReadOnly]
        public Vector2 gridPosition;

        [Header("Building Stats")]
        private int id;

        [SerializeField, ReadOnly]
        private int level = 1;

        [SerializeField, ReadOnly]
        private BuildingConfigSO _buildingConfigSO;

        [Header("Building Size")]
        public Vector2 size = new Vector2(3, 3); // Size in grid cells

        public BuildingVisual BuildingVisual => _buildingVisual;
        private BuildingVisual _buildingVisual;

        private BuildingTimer _buildingTimer;

        public int Id => id;
        public int Level => level;
        public BuildingConfigSO BuildingConfigSO => _buildingConfigSO;

        // For moving building
        private Vector2 originalGridPosition;
        private Vector2 offset;

        public Action OnInitialized { get; set; }

        private void Awake()
        {
            _buildingVisual = GetComponent<BuildingVisual>();
            _buildingTimer = GetComponent<BuildingTimer>();

            _buildingTimer.OnTimerEnded += CompleteUpgrade;
        }

        public void SetInfo(BuildingData buildingData)
        {
            this.id = buildingData.id;
            this.level = buildingData.level;
            this.Type = buildingData.type;
            this.gridPosition = new Vector2(buildingData.x, buildingData.y);

            SetGridPosition(gridPosition);

            LoadConfig();

            UpdateVisual();

            Debug.Log($"Building {id} initialized");
            OnInitialized?.Invoke();
        }

        private void UpdateVisual()
        {
            _buildingVisual.Init();
        }

        private void LoadConfig()
        {
            _buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>($"{Type}_{level}");
        }

        public BuildingData GetData()
        {
            var remainingTime = _buildingTimer.GetRemainingTime();
            return new BuildingData(id, Type, level, gridPosition, remainingTime);
        }

        #region Select/ Deselect
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
        #endregion

        #region Moving
        public bool IsCellPositionInBuilding(Vector2 cellPos)
        {
            return cellPos.x >= gridPosition.x
                && cellPos.x < gridPosition.x + size.x
                && cellPos.y >= gridPosition.y
                && cellPos.y < gridPosition.y + size.y;
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

        public void StartMoving(Vector2 startCellPos)
        {
            originalGridPosition = gridPosition;
            offset = startCellPos - gridPosition;
        }

        public void MoveTo(Vector2 cellPos)
        {
            SetGridPosition(cellPos - offset);

            if (BuildingManager.Instance.CanPlaceBuilding(this) == false)
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
            if (BuildingManager.Instance.CanPlaceBuilding(this) == true)
            {
                SetGridPosition(originalGridPosition);
            }
            Deselect();
            DataManager.Instance.SaveBuilding(this);

            GameManager.Instance.Log("Stop moving");
        }
        #endregion

        #region Build/Upgrade
        public void StartUpgrade()
        {
            var nextLevel = level + 1;
            var nextBuildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>(
                $"{Type}_{nextLevel}"
            );
            _buildingTimer.StartTimer(nextBuildingConfigSO.timeToBuild);

            _buildingVisual.HideBuildUI();
            _buildingVisual.ShowUpgradeUI();
            StopMoving();
        }

        public bool CheckUpgrading()
        {
            return _buildingTimer.IsTimerRunning;
        }

        public void CompleteUpgrade()
        {
            level++;
            DataManager.Instance.SaveBuilding(this);

            LoadConfig();
            UpdateVisual();

            _buildingVisual.HideUpgradeUI();

            BuildingManager.OnCompleteUpgrade?.Invoke();
        }

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            _buildingVisual = GetComponent<BuildingVisual>();
            _buildingVisual.SetSize(size);
        }
#endif
    }
}
