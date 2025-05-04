namespace AILibraryForNPC.Core
{
    public abstract class BaseSensor_v2
    {
        protected BaseAgent _agent;

        public void Initialize(BaseAgent agent)
        {
            _agent = agent;
        }

        public abstract void Observe(WorldState_v2 worldstate);
    }
}
