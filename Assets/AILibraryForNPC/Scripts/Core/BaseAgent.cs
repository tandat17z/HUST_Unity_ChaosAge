using System;
using UnityEngine;

namespace AILibraryForNPC.Core
{
    public abstract class BaseAgent : MonoBehaviour
    {
        protected PerceptionSystem_v2 perceptionSystem;
        protected ActionSystem_v2 actionSystem;
        protected WorldState_v2 worldState;

        protected BaseAction_v2 currentAction;

        public bool IsStart = false;

        private bool _isCompleteInitialize = false;

        public virtual void Start()
        {
            // Initialize perception system
            perceptionSystem = new PerceptionSystem_v2();
            RegisterSensors();
            perceptionSystem.InitializeSensors(this);

            // Initialize action system
            actionSystem = GetComponent<ActionSystem_v2>();
            RegisterActions();
            actionSystem.InitializeActions(this);

            OnAwake();
            _isCompleteInitialize = true;
        }

        public abstract void OnAwake();

        public abstract void RegisterSensors();
        public abstract void RegisterActions();

        void Update()
        {
            if (IsStart && _isCompleteInitialize)
            {
                UpdatePerceptionSystem();
                UpdateActionSystem();
            }
        }

        private void UpdatePerceptionSystem()
        {
            worldState = perceptionSystem.Observe();
        }

        protected virtual void UpdateActionSystem()
        {
            if (currentAction != null)
            {
                if (currentAction.IsComplete(worldState) == true)
                {
                    currentAction.PostPerform(worldState);
                    currentAction = null;
                    return;
                }
                currentAction.Perform(worldState);
                return;
            }

            currentAction = actionSystem.SelectAction(worldState);
            if (currentAction != null)
            {
                // Debug.LogWarning("currentAction: " + currentAction.GetType().Name);
                currentAction.PrePerform(worldState);
            }
        }

        public WorldState_v2 GetWorldState()
        {
            return worldState;
        }
    }
}
