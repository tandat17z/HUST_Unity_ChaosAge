using System;

namespace AILibraryForNPC.Core
{
    public abstract class BaseSensor_v2
    {
        protected BaseAgent agent;

        public void Initialize(BaseAgent agent)
        {
            this.agent = agent;
        }

        public abstract void Observe(WorldState_v2 worldstate);

        public virtual void InitializeWorldState(WorldState_v2 worldState)
        {
            return;
        }
    }
}
