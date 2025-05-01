using AILibraryForNPC.core;
using AILibraryForNPC.Modules.RL;

public class RLPerceptionSystem : PerceptionSystem
{
    protected override void OnAwake()
    {
        worldState = new BarbarianWorldState();
    }
}
