using System.Collections.Generic;
using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.QLearning
{
    public class QLearningActionSystem : ActionSystem_v2
    {
        [SerializeField]
        public bool IsTraining = true;
        public static Dictionary<string, float> qTable = new Dictionary<string, float>();

        [SerializeField]
        private float learningRate = 0.1f;

        [SerializeField]
        private float discountFactor = 0.9f;

        [SerializeField]
        private float explorationRate = 0.1f;

        private string _lastStateKey;
        private int _lastAction = -1;
        private float _reward = 0f;

        public override BaseAction_v2 SelectAction(WorldState_v2 worldState)
        {
            // Chọn action dựa trên Q-learning
            if (IsTraining && _lastStateKey != null && _lastAction != -1)
            {
                UpdateQValue(_lastStateKey, _lastAction, _reward, worldState.GetStateKey());
            }

            int index = ChooseAction(worldState);
            return _actions[index];
        }

        private void UpdateQValue(
            string lastStateKey,
            int lastAction,
            float reward,
            string currentStateKey
        )
        {
            string key = $"{lastStateKey}_{lastAction}";
            float currentQ = GetQValue(lastStateKey, lastAction);

            // Tìm Q-value lớn nhất cho next state
            float maxNextQ = float.MinValue;
            for (int i = 0; i < _actions.Count; i++)
            {
                float nextQ = GetQValue(currentStateKey, i);
                maxNextQ = Mathf.Max(maxNextQ, nextQ);
            }

            // Cập nhật Q-value theo công thức Q-learning
            float newQ = currentQ + learningRate * (reward + discountFactor * maxNextQ - currentQ);
            qTable[key] = newQ;
        }

        public void AddReward(float reward)
        {
            _reward += reward;
        }

        private int ChooseAction(WorldState_v2 worldState)
        {
            // Exploration
            if (IsTraining && Random.value < explorationRate)
            {
                return Random.Range(0, _actions.Count);
            }

            // Exploitation
            float maxQ = float.MinValue;
            int bestAction = 0;

            for (int i = 0; i < _actions.Count; i++)
            {
                float qValue = GetQValue(worldState.GetStateKey(), i);
                if (qValue > maxQ)
                {
                    maxQ = qValue;
                    bestAction = i;
                }
            }

            return bestAction;
        }

        private float GetQValue(string state, int action)
        {
            string key = $"{state}_{action}";
            if (!qTable.ContainsKey(key))
            {
                qTable[key] = 0f;
            }
            return qTable[key];
        }
    }
}
