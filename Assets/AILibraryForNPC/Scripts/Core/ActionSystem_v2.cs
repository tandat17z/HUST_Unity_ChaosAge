using System;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.Core
{
    public abstract class ActionSystem_v2 : MonoBehaviour
    {
        protected List<BaseAction_v2> _actions = new();

        void Awake()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public void InitializeActions(BaseAgent agent)
        {
            foreach (var action in _actions)
            {
                action.Initialize(agent);
            }

            Debug.LogWarning("Complete Register: " + _actions.Count);
        }

        public void AddAction(BaseAction_v2 action)
        {
            _actions.Add(action);
        }

        public abstract BaseAction_v2 SelectAction(WorldState_v2 worldState);
    }
}
