using UnityEngine;

namespace AILibraryForNPC.Core
{
    public abstract class BaseAgent : MonoBehaviour
    {
        private PerceptionSystem_v2 _perceptionSystem;
        protected ActionSystem_v2 _actionSystem;
        private WorldState_v2 _worldState;

        private BaseAction_v2 _currentAction;

        public bool IsStart { get; set; } = false;

        public BaseAgent()
        {
            // Initialize perception system
            _perceptionSystem = new PerceptionSystem_v2();
            RegisterSensors();
            _perceptionSystem.InitializeSensors(this);

            // Initialize action system
            _actionSystem = GetComponent<ActionSystem_v2>();
            RegisterActions();
            _actionSystem.InitializeActions(this);
        }

        public abstract void RegisterSensors();
        public abstract void RegisterActions();

        void Update()
        {
            if (IsStart)
            {
                _worldState = _perceptionSystem.Observe();
                if (_currentAction != null)
                {
                    if (_currentAction.IsComplete(_worldState) == true)
                    {
                        _currentAction.PostPerform(_worldState);
                        _currentAction = null;
                        return;
                    }
                    _currentAction.Perform(_worldState);
                    return;
                }

                _currentAction = _actionSystem.SelectAction(_worldState);
                if (_currentAction != null)
                {
                    _currentAction.PrePerform(_worldState);
                }
            }
        }
    }
}
