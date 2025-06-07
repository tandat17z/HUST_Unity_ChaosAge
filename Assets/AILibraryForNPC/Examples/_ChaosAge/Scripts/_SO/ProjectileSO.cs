using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "ChaosAge/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public Transform[] projectilePrefabs;

    private Dictionary<TargetType, BattleProjectile> _dictProjectile = new();

    public BattleProjectile GetProjectilePrefab(TargetType targetType)
    {
        if (_dictProjectile.ContainsKey(targetType) == false)
        {
            foreach (Transform t in projectilePrefabs)
            {
                var unit = t.GetComponent<BattleProjectile>();
                _dictProjectile.Add(unit.Type, unit);
            }
        }
        return _dictProjectile[targetType];
    }
}
