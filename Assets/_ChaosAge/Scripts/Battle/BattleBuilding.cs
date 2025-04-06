using ChaosAge.building;
using ChaosAge.Config;
using ChaosAge.Data;
using ChaosAge.manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuilding : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] TMP_Text text;

    [Header("")]
    [SerializeField] EBuildingType type;
    [SerializeField] int level;

    [SerializeField, ReadOnly] public BattleBuildingData battleBuidlingConfig = null;
    public float health = 0;
    public int target = -1;
    public double attackTimer = 0;
    public float percentage = 0;
    public BattleVector2 worldCenterPosition;

    private void Awake()
    {
        text.gameObject.SetActive(false);
        hpSlider.gameObject.SetActive(false);
    }


#if UNITY_EDITOR
    public void OnValidate()
    {
        battleBuidlingConfig.type = type;
        battleBuidlingConfig.level = level;
        battleBuidlingConfig = GameConfig.LoadFromFile("Assets/_ChaosAge/Config.json").GetBattleBuildingData(type, level);
        //baseArea.transform.localScale = new Vector3(battleBuidlingConfig.rows, battleBuidlingConfig.columns, 1);
    }
#endif

    public void SetInfo(BuildingData data)
    {
        type = data.type;
        level = data.level;
        battleBuidlingConfig = GameConfig.LoadFromFile("Assets/_ChaosAge/Config.json").GetBattleBuildingData(type, level);

        battleBuidlingConfig.x = data.x;
        battleBuidlingConfig.y = data.y;
    }

    public void Initialize()
    {
        health = battleBuidlingConfig.health;
        percentage = battleBuidlingConfig.percentage;
        Debug.LogWarning($"show ui {type}");
        text.gameObject.SetActive(true);
        hpSlider.gameObject.SetActive(true);
        text.text = battleBuidlingConfig.type.ToString();
        hpSlider.value = 1;
    }

    public void TakeDamage(float damage, ref AStarPathfinding.Grid grid, ref List<Tile> blockedTiles, ref float percentage)
    {
        if (health <= 0) { return; }
        health -= damage;

        hpSlider.value = health / battleBuidlingConfig.health;
        // die
        if (health < 0) { health = 0; }
        if (health <= 0)
        {
            for (int x = battleBuidlingConfig.x; x < battleBuidlingConfig.x + battleBuidlingConfig.columns; x++)
            {
                for (int y = battleBuidlingConfig.y; y < battleBuidlingConfig.y + battleBuidlingConfig.rows; y++)
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
                int attacksCount = (int)Math.Floor(attackTimer / battleBuidlingConfig.speed);
                if (attacksCount > 0)
                {
                    attackTimer -= (attacksCount * battleBuidlingConfig.speed);
                    for (int i = 1; i <= attacksCount; i++)
                    {
                        if (battleBuidlingConfig.radius > 0 && battleBuidlingConfig.rangedSpeed > 0)
                        {
                            Debug.Log("spawn projectile");
                            float distance = BattleVector2.Distance(_units[idxUnit].position, worldCenterPosition);

                            var projectile = FactoryManager.Instance.SpawnProjectile(TargetType.unit);
                            projectile.target = idxUnit;
                            projectile.timer = distance / battleBuidlingConfig.rangedSpeed;
                            projectile.damage = battleBuidlingConfig.damage;
                            projectile.splash = battleBuidlingConfig.splashRange;
                            BattleManager.Instance.Projectiles.Add(projectile);

                            projectile.Move(worldCenterPosition, _units[idxUnit].position);

                            Debug.LogWarning("spawn projectile");
                        }
                        else
                        {
                            _units[idxUnit].TakeDamage(battleBuidlingConfig.damage);
                            if (battleBuidlingConfig.splashRange > 0)
                            {
                                for (int j = 0; j < _units.Count; j++)
                                {
                                    if (j != idxUnit)
                                    {
                                        float distance = BattleVector2.Distance(_units[j].position, _units[idxUnit].position);
                                        if (distance < battleBuidlingConfig.splashRange * ConfigData.gridCellSize)
                                        {
                                            _units[j].TakeDamage(battleBuidlingConfig.damage * (1f - (distance / battleBuidlingConfig.splashRange * ConfigData.gridCellSize)));
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
