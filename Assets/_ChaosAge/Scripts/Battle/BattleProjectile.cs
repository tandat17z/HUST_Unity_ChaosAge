using ChaosAge.manager;
using DG.Tweening;
using UnityEngine;

public class BattleProjectile : MonoBehaviour
{
    public int target = -1;
    public float damage = 0;
    public float splash = 0;
    public float timer = 0;
    public TargetType type = TargetType.unit;
    public bool heal = false;

    public TargetType Type => type;

    public void Move(BattleVector2 start, BattleVector2 end)
    {
        var startPos = BuildingManager.Instance.Grid.transform.TransformPoint(new Vector3(start.x, 0, start.y));
        var endPos = BuildingManager.Instance.Grid.transform.TransformPoint(new Vector3(end.x, 0, end.y));
        transform.DOKill();
        transform.position = startPos;
        transform.DOMove(endPos, timer);

    }
}


public enum TargetType
{
    unit, building
}
