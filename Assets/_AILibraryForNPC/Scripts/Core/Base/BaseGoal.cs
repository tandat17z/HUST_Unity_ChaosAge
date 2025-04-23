using UnityEngine;

namespace AILibraryForNPC.core
{
    public abstract class BaseGoal : MonoBehaviour
    {
        public string goalName;
        public float priority;

        public abstract bool IsValid(WorldState worldState);
    }
}
