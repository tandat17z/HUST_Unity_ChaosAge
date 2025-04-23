namespace AILibraryForNPC.core
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    [RequireComponent(typeof(PerceptionSystem))]
    [RequireComponent(typeof(GoalSystem))]
    [RequireComponent(typeof(ActionSystem))]
    public class Agent : MonoBehaviour
    {
        private PerceptionSystem perceptionSystem;
        private GoalSystem goalSystem;
        private ActionSystem actionSystem;

        [SerializeField, ReadOnly]
        private BaseActionSO currentAction;

        void Start()
        {
            perceptionSystem = GetComponent<PerceptionSystem>();
            perceptionSystem.Initialize();

            goalSystem = GetComponent<GoalSystem>();
            actionSystem = GetComponent<ActionSystem>();
        }

        void Update()
        {
            // Cập nhật trạng thái thế giới
            var worldState = perceptionSystem.GetWorldState();

            // Nếu không có action nào đang thực hiện, chọn goal và action mới
            if (currentAction == null || !currentAction.IsExecuting)
            {
                var goal = goalSystem.SelectBestGoal(worldState);
                currentAction = actionSystem.GetAction(goal, worldState);
                if (currentAction != null)
                {
                    currentAction.StartExecute(this, worldState);
                }
            }

            // Thực hiện action hiện tại
            if (currentAction != null && currentAction.IsExecuting)
            {
                currentAction.ExecutePerFrame(this, worldState);
            }
        }
    }
}
