using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAPSystem
{
    public abstract class GAction : MonoBehaviour
    {
        public string actionName = "Action";
        public float cost = 1.0f;

        public WorldState[] preConditions;
        public WorldState[] afterEffects;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        public WorldStates agentBeliefs;

        public GAction()
        {
            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
        }

        public void Awake()
        {
            if (preConditions != null)
            {
                foreach (WorldState state in preConditions)
                {
                    preconditions.Add(state.key, state.value);
                }
            }
            if (afterEffects != null)
            {
                foreach (WorldState state in afterEffects)
                {
                    effects.Add(state.key, state.value);
                }
            }

            OnAwake();
        }

        protected abstract void OnAwake();

        public bool IsAchievable()
        {
            return true;
        }

        public bool IsAchievableGiven(Dictionary<string, int> conditions)
        {
            foreach (KeyValuePair<string, int> condition in preconditions)
            {
                if (!conditions.ContainsKey(condition.Key))
                    return false;
            }
            return true;
        }

        public abstract bool PrePerform();
        public abstract void PostPerform();
        public abstract void Perform();
        public abstract bool IsActionComplete();
    }
}
