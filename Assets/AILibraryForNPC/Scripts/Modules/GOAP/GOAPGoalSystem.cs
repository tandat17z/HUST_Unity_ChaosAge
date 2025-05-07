using System;
using System.Collections.Generic;
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

        public Dictionary<string, float> GetGoal()
        {
            Dictionary<string, float> goal = new Dictionary<string, float>();
            foreach (var state in _goals)
            {
                goal[state.key] = state.value;
            }
            return goal;
        }
    }
}
