using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseConfigSO", menuName = "ChaosAge/DefenseConfigSO")]
public class DefenseConfigSO : BuildingConfigSO
{
    public int damage;
    public float attackRange;
    public float attackSpeed;
    public float attackInterval;
}
