using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChaosAge.Data;
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
        public double pathTime = 0;
        public double pathTraveledTime = 0;
        public double attackTimer = 0;
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
                pathTime = path.length / (unit.moveSpeed * ConfigData.gridSize);
            }
        }

        public void AssignHealerTarget(int target, float distance)
        {
            attackTimer = 0;
            this.target = target;
            pathTraveledTime = 0;
            pathTime = distance / (unit.moveSpeed * ConfigData.gridSize);
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

