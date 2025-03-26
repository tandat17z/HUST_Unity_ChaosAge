using ChaosAge.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDataSO", menuName = "ChaosAge/UnitDataSO")]
public class UnitDataSO : ScriptableObject
{
    [SerializeField] UnitData[] unitDatas;

    public UnitData GetUnitData(EUnitType type)
    {
        foreach (var unitData in unitDatas)
        {
            if (unitData.type == type)
            {
                return unitData;
            }
        }
        return null;
    }
}
