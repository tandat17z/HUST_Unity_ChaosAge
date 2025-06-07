using System.Collections.Generic;
using ChaosAge.data;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingConfigSO", menuName = "ChaosAge/BuildingConfigSO")]
public class BuildingConfigSO : ScriptableObject
{
    public EBuildingType buildingType;
    public int level;

    [Header("Upgrade Config")]
    public int unlockedLevel;
    public int timeToBuild;
    public List<ResourceAndQuantity> costs;

    [Header("Properties")]
    public int health;
}
