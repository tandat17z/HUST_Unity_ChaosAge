using ChaosAge.AI.battle;
using ChaosAge.Battle;
using ChaosAge.building;
using ChaosAge.data;
using ChaosAge.spawner;
using DG.Tweening;
using UnityEngine;

public class BattleBuilding : MonoBehaviour
{
    private Building _building;
    private BuildingVisual _buildingVisual;
    public float health = 0;

    public EBuildingType Type => _building.Type;

    // public int target = -1;
    // public double attackTimer = 0;
    public float percentage = 0;

    private Tween _tween;

    public void Init()
    {
        _building = GetComponent<Building>();
        _buildingVisual = GetComponent<BuildingVisual>();
        if (_building.BuildingConfigSO == null)
            return;
        health = _building.BuildingConfigSO.health;

        // _buildingVisual.ShowInfoUI();
        // _buildingVisual.ShowSliderUI();
    }

    public void TakeDamage(float damage)
    {
        if (_tween != null) _tween.Kill();

        _buildingVisual.ShowSliderUI();
        _tween = DOVirtual.DelayedCall(5f, () =>
        {
            _buildingVisual.HideSliderUI();
        });

        health -= damage;
        if (health <= 0)
        {
            AIBattleManager.Instance.RemoveBuilding(this);
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

    private void Update()
    {
        if (_buildingVisual != null && _building != null && _building.BuildingConfigSO != null)
        {
            _buildingVisual.SetTime(health, _building.BuildingConfigSO.health);
        }
    }
}
