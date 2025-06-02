using System.Collections;
using System.Collections.Generic;
using ChaosAge.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfigSO", menuName = "ChaosAge/GameConfigSO")]
public class GameConfigSO : ScriptableObject
{
    public List<BuildingConfigSO> buildingConfigs;
}
