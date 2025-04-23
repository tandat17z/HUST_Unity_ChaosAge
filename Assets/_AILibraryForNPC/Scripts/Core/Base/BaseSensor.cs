using AILibraryForNPC.core;
using UnityEngine;

namespace AILibraryForNPC.core.Base
{
    public abstract class BaseSensor : MonoBehaviour
    {
        protected Agent agent;
        protected WorldState worldState;

        protected virtual void Awake()
        {
            agent = GetComponent<Agent>();
        }

        public virtual void Initialize(WorldState state)
        {
            worldState = state;
        }

        public abstract void UpdateSensor();
    }
}
