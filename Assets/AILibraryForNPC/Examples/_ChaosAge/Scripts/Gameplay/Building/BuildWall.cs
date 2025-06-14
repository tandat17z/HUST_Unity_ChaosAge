namespace ChaosAge.building
{
    using System;
    using ChaosAge.data;
    using ChaosAge.manager;
    using DatSystem;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.VFX;

    public class BuildWall : MonoBehaviour
    {
        private Building _building;

        void Awake()
        {
            _building = GetComponent<Building>();
            _building.OnCompleteUpgrade += OnCompleteUpgrade;
        }

        private void OnCompleteUpgrade()
        {
            if(_building.Level > 1) return;
            var pos = _building.gridPosition;
            if (BuildingManager.Instance.CanUpgradeBuilding(EBuildingType.Wall, 0))
            {
                BuildingManager.Instance.CreateBuilding(EBuildingType.Wall, false, (int)pos.x + 1, (int)pos.y);
            }
            else
            {
                GameManager.Instance.Log("Cannot create wall");
            }
        }
    }
}
