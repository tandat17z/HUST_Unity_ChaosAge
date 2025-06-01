namespace ChaosAge
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ChaosAge.Config;
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

        public int Id => id;
        public int Level => level;
        public int MaxLevel => maxLevel;
        public int Health => health;
        public int BuildCost => buildCost;
        public int UpgradeCost => upgradeCost;
        public bool IsSelected => isSelected;
        public bool IsMoving => isMoving;

        private bool isSelected = false;
        private bool isMoving = false;
        private BuildingVisual buildingVisual;

        private void Awake()
        {
            buildingVisual = GetComponent<BuildingVisual>();
        }

        public void SetInfo(int id, int level)
        {
            this.id = id;
            this.level = level;
        }

        public void PlacedOnGrid(int x, int y)
        {
            SetGridPosition(new Vector2(x, y));
        }

        public void Select()
        {
            isSelected = true;
            buildingVisual?.OnBuildingSelected();
        }

        public void Deselect()
        {
            isSelected = false;
            isMoving = false;
            buildingVisual?.OnBuildingDeselected();
        }

        public void StartMoving()
        {
            isMoving = true;
        }

        public void StopMoving()
        {
            isMoving = false;
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
                health += 100; // Example health increase per level
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
    }
}
