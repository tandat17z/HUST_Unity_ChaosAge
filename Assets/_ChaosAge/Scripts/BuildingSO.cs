using System.Collections;
using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "ChaosAge/BuildingSO")]

public class BuildingSO : ScriptableObject
{
    public Transform[] buildingPrefabs;

    private Dictionary<EBuildingType, Building> _dictBuilding = new();

    public Building GetBuilingPrefab(EBuildingType buildingType)
    {
        if (_dictBuilding.ContainsKey(buildingType) == false)
        {
            foreach (Transform t in buildingPrefabs)
            {
                var building = t.GetComponent<Building>();
                _dictBuilding.Add(building.Type, building);
            }
        }
        return _dictBuilding[buildingType];
    }
}
