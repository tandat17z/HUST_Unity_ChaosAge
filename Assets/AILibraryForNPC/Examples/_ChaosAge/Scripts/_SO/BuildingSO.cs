namespace ChaosAge.SO
{
    using System.Collections.Generic;
    using ChaosAge.building;
    using ChaosAge.data;
    using UnityEngine;

    [CreateAssetMenu(fileName = "BuildingSO", menuName = "ChaosAge/BuildingSO")]
    public class BuildingSO : ScriptableObject
    {
        public Transform[] buildingPrefabs;

        private Dictionary<EBuildingType, Building> _dictBuildingPrefab = new();

        public Building GetBuildingPrefab(EBuildingType buildingType)
        {
            if (_dictBuildingPrefab.ContainsKey(buildingType) == false)
            {
                _dictBuildingPrefab.Clear();
                foreach (Transform t in buildingPrefabs)
                {
                    var building = t.GetComponent<Building>();
                    if (_dictBuildingPrefab.ContainsKey(building.Type) == false)
                    {
                        _dictBuildingPrefab.Add(building.Type, building);
                    }
                }
            }
            return _dictBuildingPrefab[buildingType];
        }
    }
}
