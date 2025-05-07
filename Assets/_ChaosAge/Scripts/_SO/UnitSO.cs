using System.Collections.Generic;
using ChaosAge.Battle;
using ChaosAge.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSO", menuName = "ChaosAge/UnitSO")]
public class UnitSO : ScriptableObject
{
    public Transform[] buildingPrefabs;

    private Dictionary<EUnitType, BattleUnit> _dictUnit = new();

    public BattleUnit GetUnitPrefab(EUnitType unitType)
    {
        if (_dictUnit.ContainsKey(unitType) == false)
        {
            foreach (Transform t in buildingPrefabs)
            {
                var unit = t.GetComponent<BattleUnit>();
                _dictUnit.Add(unit.Type, unit);
            }
        }
        return _dictUnit[unitType];
    }
}
