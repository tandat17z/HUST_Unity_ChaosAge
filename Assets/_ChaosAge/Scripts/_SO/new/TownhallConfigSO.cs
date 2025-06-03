using System;
using System.Collections.Generic;
using ChaosAge.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "TownhallConfigSO", menuName = "ChaosAge/TownhallConfigSO")]
public class TownhallConfigSO : BuildingConfigSO
{
    public List<ResourceAndQuantity> capacities;
    public List<BuildingAndQuantity> limitBuildingNumber;

    public int GetCapacity(EResourceType resourceType)
    {
        return capacities.Find(capacity => capacity.resourceType == resourceType).quantity;
    }

    public int GetLimitBuildingNumber(EBuildingType buildingType)
    {
        try
        {
            return limitBuildingNumber
                .Find(building => building.buildingType == buildingType)
                .quantity;
        }
        catch (Exception e)
        {
            Debug.LogError("GetLimitBuildingNumber: " + e.Message);
            return 0;
        }
    }
}

[System.Serializable]
public class ResourceAndQuantity
{
    public EResourceType resourceType;
    public int quantity;
}

[System.Serializable]
public class BuildingAndQuantity
{
    public EBuildingType buildingType;
    public int quantity;
}
