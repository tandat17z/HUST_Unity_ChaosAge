using ChaosAge.Data;
using ChaosAge.manager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuilding : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] TMP_Text text;

    public BattleBuildingData building = null;
    public float health = 0;
    public int target = -1;
    public double attackTimer = 0;
    public float percentage = 0;
    public BattleVector2 worldCenterPosition;

    private void Start()
    {
        text.gameObject.SetActive(false);
        hpSlider.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        health = building.health;
        percentage = building.percentage;

        text.gameObject.SetActive(true);
        hpSlider.gameObject.SetActive(true);
        text.text = building.type.ToString();
        hpSlider.value = 1;
    }

    public void TakeDamage(float damage, ref AStarPathfinding.Grid grid, ref List<Tile> blockedTiles, ref float percentage)
    {
        if (health <= 0) { return; }
        health -= damage;

        hpSlider.value = health / building.health;
        // die
        if (health < 0) { health = 0; }
        if (health <= 0)
        {
            for (int x = building.x; x < building.x + building.columns; x++)
            {
                for (int y = building.y; y < building.y + building.rows; y++)
                {
                    grid[x, y].Blocked = false;
                    for (int i = 0; i < blockedTiles.Count; i++)
                    {
                        if (blockedTiles[i].position.x == x && blockedTiles[i].position.y == y)
                        {
                            blockedTiles.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (this.percentage > 0)
            {
                percentage += this.percentage;
            }

            Destroy(gameObject);
        }
    }

    public void HandleBuilding(int index, float deltaTime)
    {
        var _units = BattleManager.Instance.Units;
        var idxUnit = target;
        if (idxUnit >= 0)
        {
            // Nếu the building's target is dead  or Không nằm trong phạm vi bắn
            if (_units[idxUnit].health <= 0 || !BattleManager.Instance.IsUnitInRange(idxUnit, index) || (_units[idxUnit].unit.movement == UnitMoveType.underground && _units[idxUnit].path != null))
            {
                // If the building's target is dead or not in range then remove it as target
                idxUnit = -1;
            }
            else
            {
                // Building has a target
                attackTimer += deltaTime;
                int attacksCount = (int)Math.Floor(attackTimer / building.speed);
                if (attacksCount > 0)
                {
                    attackTimer -= (attacksCount * building.speed);
                    for (int i = 1; i <= attacksCount; i++)
                    {
                        if (building.radius > 0 && building.rangedSpeed > 0)
                        {
                            float distance = BattleVector2.Distance(_units[idxUnit].position, worldCenterPosition);

                            var projectile = FactoryManager.Instance.SpawnProjectile(TargetType.unit);
                            projectile.target = idxUnit;
                            projectile.timer = distance / building.rangedSpeed;
                            projectile.damage = building.damage;
                            projectile.splash = building.splashRange;
                            BattleManager.Instance.Projectiles.Add(projectile);

                            projectile.Move(worldCenterPosition, _units[idxUnit].position);
                        }
                        else
                        {
                            _units[idxUnit].TakeDamage(building.damage);
                            if (building.splashRange > 0)
                            {
                                for (int j = 0; j < _units.Count; j++)
                                {
                                    if (j != idxUnit)
                                    {
                                        float distance = BattleVector2.Distance(_units[j].position, _units[idxUnit].position);
                                        if (distance < building.splashRange * ConfigData.gridCellSize)
                                        {
                                            _units[j].TakeDamage(building.damage * (1f - (distance / building.splashRange * ConfigData.gridCellSize)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if (idxUnit < 0)
        {
            // Find a new target for this building
            if (BattleManager.Instance.FindTargetForBuilding(index))
            {
                HandleBuilding(index, deltaTime);
            }
        }
    }
}
