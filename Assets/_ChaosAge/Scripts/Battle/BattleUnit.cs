using System.Collections.Generic;
using System.Linq;
using ChaosAge.AI.battle;
using ChaosAge.Config;
using ChaosAge.Data;
using ChaosAge.manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.Battle
{
    public class BattleUnit : MonoBehaviour
    {
        [SerializeField]
        Slider hpSlider;

        [SerializeField]
        TMP_Text text;

        public EUnitType Type
        {
            get => unit.type;
        }

        public UnitData unit = null;
        public float health = 0;
        public int target = -1;
        public int mainTarget = -1;
        public BattleVector2 position;
        public Path path = null;
        public float pathTime = 0;
        public float pathTraveledTime = 0;
        public float attackTimer = 0;

        // <id, distance>
        public Dictionary<int, float> resourceTargets = new Dictionary<int, float>();
        public Dictionary<int, float> defenceTargets = new Dictionary<int, float>();
        public Dictionary<int, float> otherTargets = new Dictionary<int, float>();

        //public AttackCallback attackCallback = null;
        //public IndexCallback dieCallback = null;
        //public FloatCallback damageCallback = null;
        //public FloatCallback healCallback = null;

        public void SetInfo()
        {
            hpSlider.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            health = unit.health;
            hpSlider.value = 1;
            text.text = unit.type.ToString();
        }

        public void Initialize(int x, int y) // todo
        {
            if (unit == null)
            {
                return;
            }
            position = BattleVector2.GridToWorldPosition(new BattleVector2Int(x, y));
            health = unit.health;

            hpSlider.value = 1;
            text.text = unit.type.ToString();
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
            if (health <= 0)
            {
                return;
            }
            health -= damage;
            hpSlider.value = health / unit.health;
            //if (damageCallback != null)
            //{
            //    damageCallback.Invoke((long)unit.type, damage);
            //}
            if (health < 0)
            {
                health = 0;
            }
            if (health <= 0)
            {
                //if (dieCallback != null)
                //{
                //    dieCallback.Invoke((long)unit.type);
                //}
                Destroy(gameObject);
                AIBattleManager.Instance.units.Remove(this);
            }
        }

        public void AddHealth(float hp)
        {
            health += hp;
            Debug.Log("AddHealth: " + hp + " " + health);
            health = Mathf.Min(health, 100);
            hpSlider.value = health / unit.health;
        }

        private void Update()
        {
            hpSlider.value = health / unit.health;
            text.text = unit.type.ToString();
            // var pos = BuildingManager.Instance.Grid.transform.TransformPoint(
            //     new Vector3(position.x, 0, position.y)
            // );
            // transform.position = pos;
        }

        public void HandleUnit(int index, float deltaTime)
        {
            var _buildings = BattleManager.Instance.Buildings;
            if (path != null)
            {
                // Nếu mất target
                if (target < 0 || (target >= 0 && _buildings[target].health <= 0))
                {
                    path = null;
                    target = -1;
                }
                else
                {
                    // Update unit's position based on path
                    position = MoveComponent(path, deltaTime);

                    //// Check if target is in range -> đến tầm bắn của army
                    if (
                        unit.attackRange > 0
                        && BattleManager.Instance.IsBuildingInRange(index, target)
                    )
                    {
                        path = null;
                    }
                    else
                    {
                        // check if unit reached the end of the path == army đã đi tới cuối đường
                        BattleVector2 targetPosition = BattleManager.GridToWorldPosition(
                            new BattleVector2Int(
                                path.points.Last().Location.X,
                                path.points.Last().Location.Y
                            )
                        );
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
                    if (
                        _buildings[target].battleBuidlingConfig.type == EBuildingType.Wall
                        && mainTarget >= 0
                        && _buildings[mainTarget].health <= 0
                    )
                    {
                        target = -1;
                    }
                    else
                    {
                        if (path == null)
                        {
                            AttackComponent(_buildings[target], deltaTime);
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
                FindTargetComponent(index);
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

        private BattleVector2 MoveComponent(Path path, float deltaTime)
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
            return BattleManager.GetPathPosition(path.points, (float)(pathTraveledTime / pathTime));
        }

        private void AttackComponent(BattleBuilding buildingTarget, float deltaTime)
        {
            // Attack the target
            float multiplier = 1;
            if (unit.priority != TargetPriority.all || unit.priority != TargetPriority.none)
            {
                switch (buildingTarget.battleBuidlingConfig.type)
                {
                    case EBuildingType.TownHall:
                    case EBuildingType.GoldMine:
                    case EBuildingType.GoldStorage:
                    case EBuildingType.ElixirMine:
                    case EBuildingType.ElixirStorage:
                        //case EBuildingType.darkelixirmine:
                        //case EBuildingType.darkelixirstorage:
                        if (unit.priority != Data.TargetPriority.resources)
                        {
                            multiplier = unit.priorityMultiplier;
                        }
                        break;
                    case EBuildingType.Wall:
                        if (unit.priority != Data.TargetPriority.walls)
                        {
                            multiplier = unit.priorityMultiplier;
                        }
                        break;
                    case EBuildingType.Cannon:
                    case EBuildingType.ArcherTowner:
                        //case EBuildingType.mortor:
                        //case EBuildingType.airdefense:
                        //case EBuildingType.wizardtower:
                        //case EBuildingType.hiddentesla:
                        //case EBuildingType.bombtower:
                        //case EBuildingType.xbow:
                        //case EBuildingType.infernotower:
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
                float distance = BattleVector2.Distance(
                    position,
                    buildingTarget.worldCenterPosition
                );
                // đánh xa
                if (unit.attackRange > 0 && unit.rangedSpeed > 0)
                {
                    var projectile = FactoryManager0.Instance.SpawnProjectile(TargetType.building);
                    projectile.target = target;
                    projectile.timer = distance / unit.rangedSpeed;
                    projectile.damage = unit.damage * multiplier;
                    BattleManager.Instance.Projectiles.Add(projectile);

                    //move
                    //int columns = _buildings[target].battleBuidlingConfig.columns;
                    //int rows = _buildings[target].battleBuidlingConfig.rows;
                    projectile.Move(position, buildingTarget.worldCenterPosition);
                }
                // cận chiến
                else
                {
                    var grid = BattleManager.Instance.grid;
                    var blockedTiles = BattleManager.Instance.blockedTiles;
                    var percentage = BattleManager.Instance.percentage;
                    buildingTarget.TakeDamage(
                        unit.damage * multiplier,
                        ref grid,
                        ref blockedTiles,
                        ref percentage
                    );
                }
                attackTimer -= unit.attackSpeed;

                if (unit.type == EUnitType.wallbreaker) // cảm tử quân thì phá xong sẽ chết
                {
                    TakeDamage(health);
                }
            }
        }

        private void FindTargetComponent(int index)
        {
            // Find a target and path
            BattleManager.Instance.FindTargets(index, unit.priority);
        }
    }
}
