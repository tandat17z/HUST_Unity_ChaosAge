using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.QLearning
{
    [RequireComponent(typeof(QLearningActionSystem))]
    public abstract class QLearningAgent : BaseAgent
    {
        public void AddReward(float reward)
        {
            (actionSystem as QLearningActionSystem).AddReward(reward);
        }
    }
}
