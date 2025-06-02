using ChaosAge.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingConfigSO", menuName = "ChaosAge/BuildingConfigSO")]
public class BuildingConfigSO : ScriptableObject
{
    public EBuildingType buildingType;
    public int level;
    public int costGold;
    public int costElixir;
    public int timeToBuild;
    public int health;
}
