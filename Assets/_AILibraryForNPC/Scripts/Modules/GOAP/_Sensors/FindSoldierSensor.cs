using AILibraryForNPC.core.Base;
using ChaosAge.AI.battle;
using ChaosAge.Battle;
using Sirenix.OdinInspector;
using UnityEngine;

public class FindSoldierSensor : BaseSensor
{
    [SerializeField, ReadOnly]
    public BattleUnit targetSoldier;

    private float attackRange = 10f; // Phạm vi tấn công của công trình

    public override void UpdateSensor()
    {
        // Kiểm tra nếu mục tiêu hiện tại đã ra khỏi phạm vi
        if (targetSoldier != null)
        {
            float currentDistance = Vector3.Distance(
                transform.position,
                targetSoldier.transform.position
            );
            if (currentDistance > attackRange)
            {
                targetSoldier = null;
            }
        }

        // Tìm mục tiêu mới nếu không có mục tiêu
        if (targetSoldier == null)
        {
            var soldiers = AIBattleManager.Instance.units;
            float minDistance = float.MaxValue;
            targetSoldier = null;

            if (soldiers != null)
            {
                // Debug.LogWarning("FindSoldierSensor" + soldiers.Count);
                foreach (var soldier in soldiers)
                {
                    if (soldier != null)
                    {
                        var distance = Vector3.Distance(
                            transform.position,
                            soldier.transform.position
                        );

                        // Debug.LogWarning("FindSoldierSensor" + soldier.name + " " + distance);
                        // Debug.LogWarning("Sensor" + soldier.name + " " + distance);

                        // Chỉ chọn mục tiêu trong phạm vi tấn công
                        if (distance < minDistance && distance <= attackRange)
                        {
                            // Debug.LogWarning("SetState FindSoldierSensor" + targetSoldier.name);
                            minDistance = distance;
                            targetSoldier = soldier;
                        }
                    }
                }
            }
        }

        if (targetSoldier != null)
        {
            worldState.states["hasUnitInRange"] = 1;
        }
        else
        {
            worldState.RemoveState("hasUnitInRange");
        }
    }
}
