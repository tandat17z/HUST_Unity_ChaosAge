using AILibraryForNPC.core;
using UnityEngine;

namespace AILibraryForNPC.Modules.RL
{
    public class RLRewardSystem : MonoBehaviour
    {
        [System.Serializable]
        public class RewardCondition
        {
            public string stateKey;
            public int targetValue;
            public float rewardValue;
        }

        [SerializeField]
        private RewardCondition[] rewardConditions;

        public float CalculateReward(WorldState worldState)
        {
            float totalReward = 0f;

            foreach (var condition in rewardConditions)
            {
                if (worldState.ContainsKey(condition.stateKey))
                {
                    int currentValue = (int)worldState.GetState(condition.stateKey);
                    if (currentValue == condition.targetValue)
                    {
                        totalReward += condition.rewardValue;
                    }
                }
            }

            return totalReward;
        }
    }
}
