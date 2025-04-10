namespace AILibraryForNPC.core
{
    using UnityEngine;

    public abstract class BaseActionSO : ScriptableObject
    {
        public abstract void Execute(Agent agent);
    }
}
