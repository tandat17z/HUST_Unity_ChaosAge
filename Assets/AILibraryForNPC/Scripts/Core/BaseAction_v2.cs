namespace AILibraryForNPC.Core
{
    public abstract class BaseAction_v2
    {
        protected BaseAgent agent;

        public void Initialize(BaseAgent agent)
        {
            this.agent = agent;
        }

        public abstract void PrePerform(WorldState_v2 worldState);
        public abstract void Perform(WorldState_v2 worldState);
        public abstract void PostPerform(WorldState_v2 worldState);
        public abstract bool IsComplete(WorldState_v2 worldState);
    }
}
