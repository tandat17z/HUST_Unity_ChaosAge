using System;
using System.Collections.Generic;
using AILibraryForNPC.Core;
using ChaosAge.AI.battle;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPGoalSystem : MonoBehaviour
    {
        [Serializable]
        public class GOAPState
        {
            public string key;
            public float value;
        }

        [SerializeField]
        private List<GOAPState> _goals;

        public Dictionary<string, float> GetCurrentGoal(WorldState_v2 worldState)
        {
            var disMin = float.MaxValue;
            BattleBuilding goal = null;
            foreach (var building in AIBattleManager.Instance.buildings)
            {
                var dis = Vector3.Distance(building.transform.position, transform.position);
                if (dis < disMin)
                {
                    disMin = dis;
                    goal = building;
                }
            }
            return new Dictionary<string, float>
            {
                { "goalX", goal.transform.position.x },
                { "goalY", goal.transform.position.z },
            };
        }
    }
}
