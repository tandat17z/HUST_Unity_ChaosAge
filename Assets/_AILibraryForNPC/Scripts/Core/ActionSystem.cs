using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public abstract class ActionSystem : MonoBehaviour
    {
        public Agent agent;
        protected List<BaseAction> availableActions = new List<BaseAction>();

        void Awake()
        {
            agent = GetComponent<Agent>();
            availableActions.AddRange(GetComponents<BaseAction>());
            Initialize();
        }

        public abstract void Initialize();

        public abstract BaseAction GetAction(BaseGoal goal, WorldState worldState);

        public List<BaseAction> GetAvailableActions()
        {
            return new List<BaseAction>(availableActions);
        }
    }
}
