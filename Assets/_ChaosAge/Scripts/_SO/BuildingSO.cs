using System;
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
    //private Dictionary<EBuildingType, BattleBuilding> _dictBattleBuilding = new();

    public Building GetBuilingPrefab(EBuildingType buildingType)
    {
        if (_dictBuilding.ContainsKey(buildingType) == false)
        {
            Debug.Log($"Khong chua key {buildingType}");
            _dictBuilding.Clear();
            foreach (Transform t in buildingPrefabs)
            {
                var building = t.GetComponent<Building>();
                if (_dictBuilding.ContainsKey(building.Type) == false)
                {
                    _dictBuilding.Add(building.Type, building);
                }
                else
                {
                    Debug.Log($"Có key {building.Type}");
                }
            }
        }
        return _dictBuilding[buildingType];
    }

    //public BattleBuilding GetBattleBuilingPrefab(EBuildingType type)
    //{
    //    if (_dictBattleBuilding.ContainsKey(type) == false)
    //    {
    //        foreach (Transform t in buildingPrefabs)
    //        {
    //            var building = t.GetComponent<BattleBuilding>();
    //            _dictBattleBuilding.Add(building.type, building);
    //        }
    //    }
    //    return _dictBattleBuilding[type];
    //}
}
