using AILibraryForNPC.Core;

namespace AILibraryForNPC.Modules.QLearning
{
    public abstract class QLearningAction : BaseAction_v2
    {
        public void AddReward(float reward)
        {
            (agent as QLearningAgent).AddReward(reward);
        }
    }
}
