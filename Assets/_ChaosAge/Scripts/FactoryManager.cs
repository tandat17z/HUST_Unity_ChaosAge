namespace ChaosAge.manager
{
    using ChaosAge.Battle;
    using ChaosAge.building;
    using ChaosAge.Config;
    using ChaosAge.Data;
    using DatSystem.utils;
    using UnityEngine;

    public class FactoryManager : Singleton<FactoryManager>
    {
        protected override void OnAwake()
        {
        }

        [SerializeField] Transform container;
        [SerializeField] BuildingSO buildingSO;
        [SerializeField] UnitSO unitSO;
        [SerializeField] ProjectileSO projecctileSO;

        public Building SpawnBuilding(EBuildingType buildingType)
        {
            var prefab = buildingSO.GetBuilingPrefab(buildingType);

            var spawned = Instantiate(prefab, container);
            var building = spawned.GetComponent<Building>();
            return building;
        }

        public BattleUnit SpawnUnit(EUnitType unitType)
        {
            var prefab = unitSO.GetUnitPrefab(unitType);

            var spawned = Instantiate(prefab, container);
            var unit = spawned.GetComponent<BattleUnit>();
            return unit;
        }

        public BattleProjectile SpawnProjectile(TargetType targetType)
        {
            var prefab = projecctileSO.GetProjectilePrefab(targetType);

            var spawned = Instantiate(prefab, container);
            var unit = spawned.GetComponent<BattleProjectile>();
            return unit;
        }
    }
}

