using AILibraryForNPC.core;

namespace AILibraryForNPC.Core
{
    public abstract class BaseAction_v2
    {
        private BaseAgent _agent;

        public void Initialize(BaseAgent agent)
        {
            _agent = agent;
        }

        public abstract void PrePerform(WorldState_v2 worldState);
        public abstract void Perform(WorldState_v2 worldState);
        public abstract void PostPerform(WorldState_v2 worldState);
        public abstract bool IsComplete(WorldState_v2 worldState);
    }
}
