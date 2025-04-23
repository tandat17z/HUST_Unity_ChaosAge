using System;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public abstract class GOAPGoal : BaseGoal
    {
        [SerializeField]
        private List<TargetState> listTargetStates;
        protected Dictionary<string, int> targetStates;

        protected virtual void Start()
        {
            targetStates = new Dictionary<string, int>();
            foreach (var state in listTargetStates)
            {
                targetStates.Add(state.key, state.value);
            }
        }

        public Dictionary<string, int> GetTargetState()
        {
            return targetStates;
        }
    }

    [Serializable]
    public class TargetState
    {
        public string key;
        public int value;
    }
}
