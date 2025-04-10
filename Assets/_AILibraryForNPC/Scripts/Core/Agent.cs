namespace AILibraryForNPC.core
{
    using UnityEngine;

    public class Agent : MonoBehaviour
    {
        [SerializeField] private PerceptionSystem perceptionSystem;
        [SerializeField] private GoalSystem goalSystem;
        [SerializeField] private ActionSystem actionSystem;


        void Update()
        {
            var worldState = perceptionSystem.GetWorldState();
            var goal = goalSystem.SelectBestGoal(worldState);
            var action = actionSystem.GetAction(goal, worldState);
            action.Execute(this);
        }
    }
}