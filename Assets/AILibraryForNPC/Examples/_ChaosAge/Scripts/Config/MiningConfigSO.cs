using System.Collections.Generic;
using ChaosAge.data;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningConfigSO", menuName = "ChaosAge/MiningConfigSO")]
public class MiningConfigSO : BuildingConfigSO
{
    public float productionPerMinute;
    public ResourceAndQuantity capacity;
}
