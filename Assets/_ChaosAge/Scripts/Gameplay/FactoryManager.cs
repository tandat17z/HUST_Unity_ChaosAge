namespace ChaosAge
{
    using ChaosAge.Config;
    using DatSystem.utils;
    using UnityEngine;

    public class FactoryManager : Singleton<FactoryManager>
    {
        protected override void OnAwake() { }

        [SerializeField]
        private Transform container;

        [SerializeField]
        private BuildingSO buildingSO;

        public Building SpawnBuilding(EBuildingType buildingType)
        {
            var prefab = buildingSO.GetBuildingPrefab(buildingType);
            var spawned = Instantiate(prefab, container);
            return spawned;
        }
    }
}
