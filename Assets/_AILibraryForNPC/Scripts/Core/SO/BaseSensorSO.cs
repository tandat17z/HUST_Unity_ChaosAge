namespace AILibraryForNPC.core
{
    using UnityEngine;

    public abstract class BaseSensorSO : ScriptableObject
    {
        public abstract void UpdateSensor(Agent agent, WorldState state);
    }
}

