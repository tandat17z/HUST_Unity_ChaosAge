using ChaosAge;
using ChaosAge.AI.battle;
using ChaosAge.Battle;
using ChaosAge.building;
using ChaosAge.data;
using ChaosAge.spawner;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuilding : MonoBehaviour
{
    private Building _building;
    private BuildingVisual _buildingVisual;
    public float health = 0;

    public EBuildingType Type => _building.Type;

    // public int target = -1;
    // public double attackTimer = 0;
    public float percentage = 0;

    // void Update()
    // {
    //     _buildingVisual.SetTime(health, _building.BuildingConfigSO.health);
    // }

    public void Init()
    {
        _building = GetComponent<Building>();
        _buildingVisual = GetComponent<BuildingVisual>();
        if (_building.BuildingConfigSO == null)
            return;
        health = _building.BuildingConfigSO.health;
        Debug.LogWarning($"show ui {_building.Type}");
        _buildingVisual.ShowInfoUI();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            AIBattleManager.Instance.buildings.Remove(this);
        }
    }

    public void SpawnProjectile(BattleUnit unit)
    {
        Debug.Log("spawn projectile");
        var projectile = FactoryManager.Instance.SpawnProjectile(TargetType.unit);
        // BattleManager.Instance.Projectiles.Add(projectile);

        projectile.Move(
            transform.position,
            unit.transform.position,
            () =>
            {
                unit.TakeDamage(10);
            }
        );
    }
}
