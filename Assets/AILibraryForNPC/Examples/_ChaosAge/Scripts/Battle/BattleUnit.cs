using System.Collections.Generic;
using System.Linq;
using ChaosAge.AI.battle;
using ChaosAge.data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.Battle
{
    public class BattleUnit : MonoBehaviour
    {
        public EUnitType Type
        {
            get => unitConfig.type;
        }

        public UnitConfigSO unitConfig = null;
        private VisualUnit _visualUnit;
        public float Health => _health;
        private float _health;

        private void Awake()
        {
            _visualUnit = GetComponent<VisualUnit>();
        }

        public void SetInfo() { }

        public void Initialize(Vector2 cell) // todo
        {
            _health = unitConfig.health;
            _visualUnit.SetHealth((int)Health, unitConfig.health);

            transform.position = AIBattleManager.Instance.GetWorldPosition(cell);
        }

        public void TakeDamage(float damage)
        {
            if (_health <= 0)
            {
                return;
            }
            _health -= damage;
            //if (damageCallback != null)
            //{
            //    damageCallback.Invoke((long)unit.type, damage);
            //}
            if (_health < 0)
            {
                _health = 0;
            }
            if (_health <= 0)
            {
                //if (dieCallback != null)
                //{
                //    dieCallback.Invoke((long)unit.type);
                //}
                Destroy(gameObject);
                AIBattleManager.Instance.units.Remove(this);
            }

            _visualUnit.SetHealth((int)_health, unitConfig.health);
        }

        public void AddHealth(float hp)
        {
            _health += hp;
            Debug.Log("AddHealth: " + hp + " " + _health);
            _health = Mathf.Min(_health, 100);
        }

        // public void HandleUnit(int index, float deltaTime)
        // {
        //     var _buildings = BattleManager.Instance.Buildings;
        //     if (path != null)
        //     {
        //         // Nếu mất target
        //         if (target < 0 || (target >= 0 && _buildings[target].health <= 0))
        //         {
        //             path = null;
        //             target = -1;
        //         }
        //         else
        //         {
        //             // Update unit's position based on path
        //             position = MoveComponent(path, deltaTime);

        //             //// Check if target is in range -> đến tầm bắn của army
        //             if (
        //                 unit.attackRange > 0
        //                 && BattleManager.Instance.IsBuildingInRange(index, target)
        //             )
        //             {
        //                 path = null;
        //             }
        //             else
        //             {
        //                 // check if unit reached the end of the path == army đã đi tới cuối đường
        //                 BattleVector2 targetPosition = BattleManager.GridToWorldPosition(
        //                     new BattleVector2Int(
        //                         path.points.Last().Location.X,
        //                         path.points.Last().Location.Y
        //                     )
        //                 );
        //                 float distance = BattleVector2.Distance(position, targetPosition);
        //                 if (distance <= ConfigData.gridCellSize * 0.05f)
        //                 {
        //                     position = targetPosition;
        //                     path = null;
        //                 }
        //             }
        //         }
        //     }

        //     if (target >= 0)
        //     {
        //         if (_buildings[target].health > 0)
        //         {
        //             if (
        //                 _buildings[target].battleBuidlingConfig.type == EBuildingType.Wall
        //                 && mainTarget >= 0
        //                 && _buildings[mainTarget].health <= 0
        //             )
        //             {
        //                 target = -1;
        //             }
        //             else
        //             {
        //                 if (path == null)
        //                 {
        //                     AttackComponent(_buildings[target], deltaTime);
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             target = -1;
        //         }
        //     }

        //     if (target < 0)
        //     {
        //         FindTargetComponent(index);
        //         if (deltaTime > 0 && target >= 0)
        //         {
        //             HandleUnit(index, deltaTime);
        //         }
        //     }
        // }

        // //// hồi máu
        // //public void Heal(float amount)
        // //{
        // //    if (amount <= 0 || health <= 0) { return; }
        // //    health += amount;
        // //    if (health > unit.health) { health = unit.health; }
        // //    if (healCallback != null)
        // //    {
        // //        healCallback.Invoke((long)unit.type, amount);
        // //    }
        // //}

        // private BattleVector2 MoveComponent(Path path, float deltaTime)
        // {
        //     float remainedTime = pathTime - pathTraveledTime;

        //     if (remainedTime >= deltaTime)
        //     {
        //         pathTraveledTime += deltaTime;
        //         deltaTime = 0;
        //     }
        //     else
        //     {
        //         pathTraveledTime = pathTime;
        //         deltaTime -= remainedTime;
        //     }
        //     return BattleManager.GetPathPosition(path.points, (float)(pathTraveledTime / pathTime));
        // }

        // private void AttackComponent(BattleBuilding buildingTarget, float deltaTime)
        // {
        //     // Attack the target
        //     float multiplier = 1;
        //     if (unit.priority != TargetPriority.all || unit.priority != TargetPriority.none)
        //     {
        //         switch (buildingTarget.battleBuidlingConfig.type)
        //         {
        //             case EBuildingType.TownHall:
        //             case EBuildingType.GoldMine:
        //             case EBuildingType.GoldStorage:
        //             case EBuildingType.ElixirMine:
        //             case EBuildingType.ElixirStorage:
        //                 //case EBuildingType.darkelixirmine:
        //                 //case EBuildingType.darkelixirstorage:
        //                 if (unit.priority != Data.TargetPriority.resources)
        //                 {
        //                     multiplier = unit.priorityMultiplier;
        //                 }
        //                 break;
        //             case EBuildingType.Wall:
        //                 if (unit.priority != Data.TargetPriority.walls)
        //                 {
        //                     multiplier = unit.priorityMultiplier;
        //                 }
        //                 break;
        //             case EBuildingType.Cannon:
        //             case EBuildingType.ArcherTowner:
        //                 //case EBuildingType.mortor:
        //                 //case EBuildingType.airdefense:
        //                 //case EBuildingType.wizardtower:
        //                 //case EBuildingType.hiddentesla:
        //                 //case EBuildingType.bombtower:
        //                 //case EBuildingType.xbow:
        //                 //case EBuildingType.infernotower:
        //                 if (unit.priority != TargetPriority.defenses)
        //                 {
        //                     multiplier = unit.priorityMultiplier;
        //                 }
        //                 break;
        //         }
        //     }
        //     attackTimer += deltaTime;

        //     if (attackTimer >= unit.attackSpeed)
        //     {
        //         float distance = BattleVector2.Distance(
        //             position,
        //             buildingTarget.worldCenterPosition
        //         );
        //         // đánh xa
        //         if (unit.attackRange > 0 && unit.rangedSpeed > 0)
        //         {
        //             var projectile = FactoryManager0.Instance.SpawnProjectile(TargetType.building);
        //             projectile.target = target;
        //             projectile.timer = distance / unit.rangedSpeed;
        //             projectile.damage = unit.damage * multiplier;
        //             BattleManager.Instance.Projectiles.Add(projectile);

        //             //move
        //             //int columns = _buildings[target].battleBuidlingConfig.columns;
        //             //int rows = _buildings[target].battleBuidlingConfig.rows;
        //             projectile.Move(position, buildingTarget.worldCenterPosition);
        //         }
        //         // cận chiến
        //         else
        //         {
        //             var grid = BattleManager.Instance.grid;
        //             var blockedTiles = BattleManager.Instance.blockedTiles;
        //             var percentage = BattleManager.Instance.percentage;
        //             buildingTarget.TakeDamage(
        //                 unit.damage * multiplier,
        //                 ref grid,
        //                 ref blockedTiles,
        //                 ref percentage
        //             );
        //         }
        //         attackTimer -= unit.attackSpeed;

        //         if (unit.type == EUnitType.wallbreaker) // cảm tử quân thì phá xong sẽ chết
        //         {
        //             TakeDamage(health);
        //         }
        //     }
        // }

        // private void FindTargetComponent(int index)
        // {
        //     // Find a target and path
        //     BattleManager.Instance.FindTargets(index, unit.priority);
        // }
    }
}
