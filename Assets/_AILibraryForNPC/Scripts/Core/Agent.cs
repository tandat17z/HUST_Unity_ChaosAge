using Sirenix.OdinInspector;
using UnityEngine;

namespace AILibraryForNPC.core
{
    [RequireComponent(typeof(PerceptionSystem))]
    public abstract class Agent : MonoBehaviour
    {
        protected PerceptionSystem perceptionSystem;
        protected GoalSystem goalSystem;
        protected ActionSystem actionSystem;

        [SerializeField, ReadOnly]
        protected BaseAction currentAction;

        protected void Start()
        {
            perceptionSystem = GetComponent<PerceptionSystem>();
            perceptionSystem.Initialize();

            goalSystem = GetComponent<GoalSystem>();
            actionSystem = GetComponent<ActionSystem>();
            Initialize();
        }

        public abstract void Initialize();

        public virtual void UpdateAgent()
        {
            var worldState = perceptionSystem.GetWorldState();
            UpdateDecisionMaking(worldState);
        }

        protected virtual void UpdateDecisionMaking(WorldState worldState)
        {
            // Kiểm tra action hiện tại
            if (currentAction != null)
            {
                if (currentAction.IsActionComplete(worldState))
                {
                    currentAction.Perform(worldState);
                    return;
                }
                else
                {
                    currentAction.PostPerform(worldState);
                    currentAction = null;
                }
            }

            // chọn thực hiện action mới
            if (currentAction == null)
            {
                var goal = goalSystem?.SelectBestGoal(worldState);
                currentAction = actionSystem?.GetAction(goal, worldState);
                if (currentAction != null)
                {
                    currentAction.PrePerform(worldState);
                }
            }
        }

        void Update()
        {
            UpdateAgent();
        }
    }
}
