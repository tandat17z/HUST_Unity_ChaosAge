using UnityEngine;

namespace AILibraryForNPC.core
{
    public abstract class BaseGoal : MonoBehaviour
    {
        public abstract string GoalName { get; }
        public abstract float Priority { get; }

        public abstract bool IsValid(WorldState worldState);
        public abstract bool IsAchieved(WorldState worldState);
    }
}
