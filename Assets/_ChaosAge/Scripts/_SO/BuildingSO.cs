using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge;
using ChaosAge.building;
using ChaosAge.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "ChaosAge/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public Transform[] buildingPrefabs;

    private Dictionary<EBuildingType, Building0> _dictBuilding = new();
    private Dictionary<EBuildingType, Building> _dictBuildingPrefab = new();

    //private Dictionary<EBuildingType, BattleBuilding> _dictBattleBuilding = new();

    public Building0 GetBuilingPrefab(EBuildingType buildingType)
    {
        if (_dictBuilding.ContainsKey(buildingType) == false)
        {
            Debug.Log($"Khong chua key {buildingType}");
            _dictBuilding.Clear();
            foreach (Transform t in buildingPrefabs)
            {
                var building = t.GetComponent<Building0>();
                if (_dictBuilding.ContainsKey(building.Type) == false)
                {
                    _dictBuilding.Add(building.Type, building);
                }
                else
                {
                    Debug.Log($"C� key {building.Type}");
                }
            }
        }
        return _dictBuilding[buildingType];
    }

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
