using System.Collections;
using System.Collections.Generic;
using ChaosAge.data;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfigSO", menuName = "ChaosAge/UnitConfigSO")]
public class UnitConfigSO : ScriptableObject
{
    public EUnitType type;
    public int health;
    public int damage;
    public int attackRange;
    public int attackSpeed;
    public int attackDelay;
}
