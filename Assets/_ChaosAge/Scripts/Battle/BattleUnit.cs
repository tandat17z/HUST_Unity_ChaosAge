using System.Collections.Generic;
using System.Linq;
using ChaosAge.Config;
using ChaosAge.Data;
using ChaosAge.manager;
using UnityEngine;

namespace ChaosAge.Battle
{
    public class BattleUnit : MonoBehaviour
    {
        public EUnitType Type { get => unit.type; }

        public UnitData unit = null;
        public float health = 0;
        public int target = -1;
        public int mainTarget = -1;
        public BattleVector2 position;
        public Path path = null;
        public float pathTime = 0;
        public float pathTraveledTime = 0;
        public float attackTimer = 0;
        public Dictionary<int, float> resourceTargets = new Dictionary<int, float>();
        public Dictionary<int, float> defenceTargets = new Dictionary<int, float>();
        public Dictionary<int, float> otherTargets = new Dictionary<int, float>();
        //public AttackCallback attackCallback = null;
        //public IndexCallback dieCallback = null;
        //public FloatCallback damageCallback = null;
        //public FloatCallback healCallback = null;

        public void Initialize(int x, int y) // todo
        {
            if (unit == null) { return; }
            position = BattleVector2.GridToWorldPosition(new BattleVector2Int(x, y));
            health = unit.health;
        }

        public Dictionary<int, float> GetAllTargets()
        {
            Dictionary<int, float> temp = new Dictionary<int, float>();
            if (otherTargets.Count > 0)
            {
                temp = temp.Concat(otherTargets).ToDictionary(x => x.Key, x => x.Value);
            }
            if (resourceTargets.Count > 0)
            {
                temp = temp.Concat(resourceTargets).ToDictionary(x => x.Key, x => x.Value);
            }
            if (defenceTargets.Count > 0)
            {
                temp = temp.Concat(defenceTargets).ToDictionary(x => x.Key, x => x.Value);
            }
            return temp;
        }
        public void AssignTarget(int target, Path path)
        {
            attackTimer = 0;
            this.target = target;
            this.path = path;
            if (path != null)
            {
                pathTraveledTime = 0;
                pathTime = path.length / (unit.moveSpeed * ConfigData.gridCellSize);
            }
        }

        public void AssignHealerTarget(int target, float distance)
        {
            attackTimer = 0;
            this.target = target;
            pathTraveledTime = 0;
            pathTime = distance / (unit.moveSpeed * ConfigData.gridCellSize);
        }

        public void TakeDamage(float damage)
        {
            if (health <= 0) { return; }
            health -= damage;
            //if (damageCallback != null)
            //{
            //    damageCallback.Invoke((long)unit.type, damage);
            //}
            if (health < 0) { health = 0; }
            if (health <= 0)
            {
                //if (dieCallback != null)
                //{
                //    dieCallback.Invoke((long)unit.type);
                //}
            }
        }

        private void Update()
        {
            var pos = BuildingManager.Instance.Grid.transform.TransformPoint(new Vector3(position.x, 0, position.y));
            transform.position = pos;
        }

        public void HandleUnit(int index, float deltaTime)
        {
            var _buildings = BattleManager.Instance.Buildings;
            if (path != null)
            {

                if (target < 0 || (target >= 0 && _buildings[target].health <= 0))
                {
                    path = null;
                    target = -1;
                }
                else
                {
                    float remainedTime = pathTime - pathTraveledTime;

                    if (remainedTime >= deltaTime)
                    {
                        pathTraveledTime += deltaTime;
                        deltaTime = 0;
                    }
                    else
                    {
                        pathTraveledTime = pathTime;
                        deltaTime -= remainedTime;
                    }

                    // Update unit's position based on path
                    position = BattleManager.GetPathPosition(path.points, (float)(pathTraveledTime / pathTime));
                    //// Check if target is in range
                    if (unit.attackRange > 0 && BattleManager.Instance.IsBuildingInRange(index, target))
                    {
                        path = null;
                    }
                    else
                    {
                        // check if unit reached the end of the path
                        BattleVector2 targetPosition = BattleManager.GridToWorldPosition(new BattleVector2Int(path.points.Last().Location.X, path.points.Last().Location.Y));
                        float distance = BattleVector2.Distance(position, targetPosition);
                        if (distance <= ConfigData.gridCellSize * 0.05f)
                        {
                            position = targetPosition;
                            path = null;
                        }
                    }
                }
            }

            if (target >= 0)
            {
                if (_buildings[target].health > 0)
                {
                    if (_buildings[target].building.type == EBuildingType.wall && mainTarget >= 0 && _buildings[mainTarget].health <= 0)
                    {
                        target = -1;
                    }
                    else
                    {
                        if (path == null)
                        {
                            // Attack the target
                            float multiplier = 1;
                            if (unit.priority != TargetPriority.all || unit.priority != TargetPriority.none)
                            {
                                switch (_buildings[target].building.type)
                                {
                                    case EBuildingType.townhall:
                                    case EBuildingType.goldmine:
                                    case EBuildingType.goldstorage:
                                    case EBuildingType.elixirmine:
                                    case EBuildingType.elixirstorage:
                                    case EBuildingType.darkelixirmine:
                                    case EBuildingType.darkelixirstorage:
                                        if (unit.priority != Data.TargetPriority.resources)
                                        {
                                            multiplier = unit.priorityMultiplier;
                                        }
                                        break;
                                    case EBuildingType.wall:
                                        if (unit.priority != Data.TargetPriority.walls)
                                        {
                                            multiplier = unit.priorityMultiplier;
                                        }
                                        break;
                                    case EBuildingType.cannon:
                                    case EBuildingType.archertower:
                                    case EBuildingType.mortor:
                                    case EBuildingType.airdefense:
                                    case EBuildingType.wizardtower:
                                    case EBuildingType.hiddentesla:
                                    case EBuildingType.bombtower:
                                    case EBuildingType.xbow:
                                    case EBuildingType.infernotower:
                                        if (unit.priority != TargetPriority.defenses)
                                        {
                                            multiplier = unit.priorityMultiplier;
                                        }
                                        break;
                                }
                            }
                            attackTimer += deltaTime;
                            if (attackTimer >= unit.attackSpeed)
                            {
                                float distance = BattleVector2.Distance(position, _buildings[target].worldCenterPosition);
                                if (unit.attackRange > 0 && unit.rangedSpeed > 0)
                                {
                                    var projectile = FactoryManager.Instance.SpawnProjectile(TargetType.building);
                                    projectile.target = target;
                                    projectile.timer = distance / unit.rangedSpeed;
                                    projectile.damage = unit.damage * multiplier;
                                    BattleManager.Instance.Projectiles.Add(projectile);

                                    //move
                                    int columns = _buildings[target].building.columns;
                                    int rows = _buildings[target].building.rows;
                                    projectile.Move(position, new BattleVector2(_buildings[target].building.x + columns / 2f, _buildings[target].building.y + rows / 2f));
                                }
                                else
                                {
                                    var grid = BattleManager.Instance.grid;
                                    var blockedTiles = BattleManager.Instance.blockedTiles;
                                    var percentage = BattleManager.Instance.percentage;
                                    _buildings[target].TakeDamage(unit.damage * multiplier, ref grid, ref blockedTiles, ref percentage);
                                }
                                attackTimer -= unit.attackSpeed;

                                if (unit.type == EUnitType.wallbreaker)
                                {
                                    TakeDamage(health);
                                }
                            }
                        }
                    }
                }
                else
                {
                    target = -1;
                }
            }

            if (target < 0)
            {
                // Find a target and path
                BattleManager.Instance.FindTargets(index, unit.priority);
                if (deltaTime > 0 && target >= 0)
                {
                    HandleUnit(index, deltaTime);
                }
            }
        }
        //// hồi máu
        //public void Heal(float amount)
        //{
        //    if (amount <= 0 || health <= 0) { return; }
        //    health += amount;
        //    if (health > unit.health) { health = unit.health; }
        //    if (healCallback != null)
        //    {
        //        healCallback.Invoke((long)unit.type, amount);
        //    }
        //}
    }
}

