using System.Collections.Generic;
using ChaosAge.Battle;
using ChaosAge.data;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialsByLevelSO", menuName = "ChaosAge/MaterialsByLevelSO")]
public class MaterialsByLevelSO : ScriptableObject
{
    public Material[] materials;

    public Material GetMaterial(int level)
    {
        return materials[level];
    }
}
