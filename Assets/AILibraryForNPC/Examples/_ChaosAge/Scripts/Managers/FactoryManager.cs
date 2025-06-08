namespace ChaosAge.spawner
{
    using System;
    using ChaosAge.Battle;
    using ChaosAge.building;
    using ChaosAge.data;
    using ChaosAge.SO;
    using DatSystem.utils;
    using UnityEngine;

    public class FactoryManager : Singleton<FactoryManager>
    {
        protected override void OnAwake() { }

        [SerializeField]
        private Transform container;

        [SerializeField]
        private BuildingSO buildingSO;

        [SerializeField]
        private UnitSO unitSO;

        public Building SpawnBuilding(EBuildingType buildingType)
        {
            var prefab = buildingSO.GetBuildingPrefab(buildingType);
            var spawned = Instantiate(prefab, container);
            return spawned;
        }

        public BattleUnit SpawnUnit(EUnitType unitType)
        {
            var prefab = unitSO.GetUnitPrefab(unitType);
            var spawned = Instantiate(prefab, container);
            return spawned;
        }

        public BattleProjectile SpawnProjectile(TargetType unit)
        {
            throw new NotImplementedException();
        }
    }
}
