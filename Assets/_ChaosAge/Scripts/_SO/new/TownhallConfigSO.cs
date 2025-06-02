using System.Collections.Generic;
using ChaosAge.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "TownhallConfigSO", menuName = "ChaosAge/TownhallConfigSO")]
public class TownhallConfigSO : BuildingConfigSO
{
    public int capacityGold;
    public int capacityElixir;
    public Dictionary<EBuildingType, int> buildingCapacity;
}
