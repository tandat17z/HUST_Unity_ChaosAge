using AILibraryForNPC.core;

namespace AILibraryForNPC.Modules.RL
{
    public abstract class RLWorldState : WorldState
    {
        public abstract string GetStateKey();
    }
}
