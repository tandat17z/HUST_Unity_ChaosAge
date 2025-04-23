using System.Collections.Generic;
using AILibraryForNPC.core;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public abstract class GOAPGoal : BaseGoal
    {
        public override string GoalName => GetType().Name;
        public override float Priority => priority;

        [SerializeField]
        protected float priority = 1.0f;

        protected Dictionary<string, int> targetStates = new Dictionary<string, int>();

        protected virtual void Awake()
        {
            Initialize();
        }

        protected abstract void Initialize();

        public override bool IsValid(WorldState worldState)
        {
            return true; // Override in derived classes if needed
        }

        public override bool IsAchieved(WorldState worldState)
        {
            return false; // Override in derived classes if needed
        }

        public Dictionary<string, int> GetTargetState()
        {
            return targetStates;
        }
    }
}
