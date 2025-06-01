namespace ChaosAge.manager
{
    using System;
    using ChaosAge.AI.battle;
    using ChaosAge.Battle;
    using ChaosAge.building;
    using ChaosAge.Config;
    using ChaosAge.Data;
    using DatSystem.utils;
    using UnityEngine;

    public class FactoryManager0 : Singleton<FactoryManager0>
    {
        protected override void OnAwake() { }

        [SerializeField]
        Transform container;

        [SerializeField]
        BuildingSO buildingSO;

        [SerializeField]
        UnitSO unitSO;

        [SerializeField]
        ProjectileSO projecctileSO;

        [SerializeField]
        AIBuildingSO aiBbuildingSO;

        public Building0 SpawnBuilding(EBuildingType buildingType)
        {
            var prefab = buildingSO.GetBuilingPrefab(buildingType);

            var spawned = Instantiate(prefab, container);
            var building = spawned.GetComponent<Building0>();
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
