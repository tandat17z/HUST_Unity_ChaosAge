using System.Collections.Generic;
using ChaosAge.data;
using UnityEngine;

[CreateAssetMenu(fileName = "StorageConfigSO", menuName = "ChaosAge/StorageConfigSO")]
public class StorageConfigSO : BuildingConfigSO
{
    public ResourceAndQuantity capacity;
}
