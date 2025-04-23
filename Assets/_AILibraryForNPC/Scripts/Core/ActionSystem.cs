using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public abstract class ActionSystem : MonoBehaviour
    {
        public Agent agent;
        private List<BaseAction> availableActions = new List<BaseAction>();

        void Start()
        {
            agent = GetComponent<Agent>();
            availableActions.AddRange(GetComponents<BaseAction>());
        }

        public abstract void Initialize();

        public abstract BaseAction GetAction(BaseGoal goal, WorldState worldState);

        protected virtual bool CanAchieveGoal(
            BaseAction action,
            BaseGoal goal,
            WorldState worldState
        )
        {
            // Override this method in derived classes to implement different goal checking strategies
            return true;
        }
    }
}
