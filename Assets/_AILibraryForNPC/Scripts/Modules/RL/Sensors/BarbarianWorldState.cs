using System;
using UnityEngine;

namespace AILibraryForNPC.Modules.RL
{
    public class BarbarianWorldState : RLWorldState
    {
        public float health;
        public float distanceToDefense;
        public float distanceToTownhall;
        public float hpDefense;
        public float hpTownhall;

        public float countUnit;

        public GameObject targetDefense;
        public GameObject targetTownhall;

        public int reward;

        public override string GetStateKey()
        {
            int levelHp = GetHpLevel(health);
            int levelDistanceDefense = GetDistanceLevel(distanceToDefense);
            int levelHpDefense = GetHpLevel(hpDefense);
            int levelDistanceTownhall = GetDistanceLevel(distanceToTownhall);
            int levelHpTownhall = GetHpLevel(hpTownhall);
            int levelCountUnit = GetCountUnitLevel(countUnit);
            return $"{levelCountUnit}_{levelHp}_{levelDistanceDefense}_{levelHpDefense}_{levelDistanceTownhall}_{levelHpTownhall}";
        }

        private int GetCountUnitLevel(float countUnit)
        {
            if (countUnit <= 3)
                return 0;
            if (countUnit <= 6)
                return 1;
            return 2;
        }

        private int GetHpLevel(float hp)
        {
            if (hp <= 30)
                return 0;
            return 1;
        }

        private int GetDistanceLevel(float distance)
        {
            if (distance <= 5)
                return 0;
            if (distance <= 10)
                return 1;
            return 2;
        }

        public GameObject GetTarget()
        {
            Debug.LogWarning("GetTarget: " + $"{distanceToTownhall}_{targetTownhall}");
            Debug.LogWarning("GetTarget: " + $"{distanceToDefense}_{targetDefense}");
            if (distanceToTownhall < 3 && targetTownhall != null)
                return targetTownhall;
            if (distanceToDefense < 3 && targetDefense != null)
                return targetDefense;
            return null;
        }

        public int GetReward()
        {
            int res = reward;
            reward = 0;
            return res;
        }
    }
}
