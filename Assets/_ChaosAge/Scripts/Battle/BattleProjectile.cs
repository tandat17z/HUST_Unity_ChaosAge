using System;
using AStarPathfinding;
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

    public void HandleProjectile(float deltaTime)
    {
        var _units = BattleManager.Instance.Units;
        var _buildings = BattleManager.Instance.Buildings;
        timer -= deltaTime;
        if (timer <= 0)
        {
            // hồi máu or gây damage
            if (type == TargetType.unit)
            {
                // hồi máu
                if (heal)
                {
                    //_units[projectiles[i].target].Heal(projectiles[i].damage);

                    //// hồi máu trong phạm vi nổ (splash)
                    //// Không hồi màu cho đối tượng bay
                    //for (int j = 0; j < _units.Count; j++)
                    //{
                    //    if (_units[j].health <= 0 || j == projectiles[i].target || _units[j].unit.movement == UnitMoveType.fly)
                    //    {
                    //        continue;
                    //    }
                    //    float distance = BattleVector2.Distance(_units[j].position, _units[projectiles[i].target].position);
                    //    if (distance < projectiles[i].splash * ConfigData.gridCellSize)
                    //    {
                    //        _units[j].Heal(projectiles[i].damage * (1f - (distance / projectiles[i].splash * ConfigData.gridCellSize)));
                    //    }
                    //}
                }
                // GÂy damage, tương tự
                else
                {
                    _units[target].TakeDamage(damage);
                    if (splash > 0)
                    {
                        for (int j = 0; j < _units.Count; j++)
                        {
                            if (j != target)
                            {
                                float distance = BattleVector2.Distance(_units[j].position, _units[target].position);
                                if (distance < splash * ConfigData.gridCellSize)
                                {
                                    _units[j].TakeDamage(damage * (1f - (distance / splash * ConfigData.gridCellSize)));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var grid = BattleManager.Instance.grid;
                var blockedTiles = BattleManager.Instance.blockedTiles;
                var percentage = BattleManager.Instance.percentage;
                _buildings[target].TakeDamage(damage, ref grid, ref blockedTiles, ref percentage);
            }
            BattleManager.Instance.Projectiles.Remove(this);
            Destroy(gameObject);
        }
    }
}


public enum TargetType
{
    unit, building
}
