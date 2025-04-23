using AILibraryForNPC.core.Base;
using ChaosAge.AI.battle;
using Sirenix.OdinInspector;
using UnityEngine;

public class FindBuildingSensor : BaseSensor
{
    [SerializeField, ReadOnly]
    public BattleBuilding targetBuilding;

    public override void UpdateSensor()
    {
        if (targetBuilding == null)
        {
            // Lấy công trình gần nhất
            var buildings = AIBattleManager.Instance.buildings;
            float minDistance = float.MaxValue;
            targetBuilding = null;
            foreach (var building in buildings)
            {
                if (building != null)
                {
                    var distance = Vector3.Distance(
                        transform.position,
                        building.transform.position
                    );
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        targetBuilding = building;
                    }
                }
            }
        }
    }
}
