using UnityEngine;

namespace AILibraryForNPC.core
{
    public abstract class BaseAction : MonoBehaviour
    {
        public string actionName;

        public abstract void PrePerform(WorldState worldState);
        public abstract void Perform(WorldState worldState);
        public abstract void PostPerform(WorldState worldState);
        public abstract bool IsActionComplete(WorldState worldState);

        public bool Equals(BaseAction other)
        {
            return actionName == other.actionName;
        }
    }
}
