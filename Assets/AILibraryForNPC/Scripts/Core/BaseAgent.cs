using UnityEngine;

namespace AILibraryForNPC.Core
{
    public abstract class BaseAgent : MonoBehaviour
    {
        protected PerceptionSystem_v2 perceptionSystem;
        protected ActionSystem_v2 actionSystem;
        protected WorldState_v2 worldState;

        private BaseAction_v2 _currentAction;

        public bool IsStart { get; set; } = false;

        void Awake()
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
        }

        public abstract void OnAwake();

        public abstract void RegisterSensors();
        public abstract void RegisterActions();

        void Update()
        {
            if (IsStart)
            {
                worldState = perceptionSystem.Observe();
                if (_currentAction != null)
                {
                    if (_currentAction.IsComplete(worldState) == true)
                    {
                        _currentAction.PostPerform(worldState);
                        _currentAction = null;
                        return;
                    }
                    _currentAction.Perform(worldState);
                    return;
                }

                _currentAction = actionSystem.SelectAction(worldState);
                Debug.LogWarning("currentAction: " + _currentAction.GetType().Name);
                if (_currentAction != null)
                {
                    _currentAction.PrePerform(worldState);
                }
            }
        }
    }
}
