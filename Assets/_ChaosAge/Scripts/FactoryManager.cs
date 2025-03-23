namespace ChaosAge.manager
{
    using ChaosAge.building;
    using ChaosAge.Config;
    using DatSystem.utils;
    using UnityEngine;

    public class FactoryManager : Singleton<FactoryManager>
    {
        protected override void OnAwake()
        {
        }

        [SerializeField] Transform container;
        [SerializeField] BuildingSO buildingSO;

        public Building SpawnBuilding(EBuildingType buildingType)
        {
            var prefab = buildingSO.GetBuilingPrefab(buildingType);

            var spawned = Instantiate(prefab, container);
            var building = spawned.GetComponent<Building>();
            return building;
        }
    }
}

