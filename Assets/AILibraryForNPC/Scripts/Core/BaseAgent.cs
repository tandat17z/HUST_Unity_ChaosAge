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
                if (_currentAction != null)
                {
                    Debug.LogWarning("currentAction: " + _currentAction.GetType().Name);
                    _currentAction.PrePerform(worldState);
                }
            }
        }
    }
}
