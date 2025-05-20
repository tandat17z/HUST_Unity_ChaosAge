using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.QLearning
{
    public class QLearningActionSystem : ActionSystem_v2
    {
        [SerializeField]
        public bool IsTraining = true;

        [SerializeField]
        private QLearningTableSO qLearningTable;

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
                Debug.LogWarning(
                    "UpdateQValue: "
                        + $"{_lastStateKey} {_lastAction} {_reward} {worldState.GetStateKey()}"
                );
                UpdateQValue(_lastStateKey, _lastAction, _reward, worldState.GetStateKey());
                ResetReward();
            }

            int index = ChooseAction(worldState);
            _lastStateKey = worldState.GetStateKey();
            _lastAction = index;
            return _actions[index];
        }

        private void UpdateQValue(
            string lastStateKey,
            int lastAction,
            float reward,
            string currentStateKey
        )
        {
            float currentQ = qLearningTable.GetQValue(lastStateKey, lastAction);

            // Tìm Q-value lớn nhất cho next state
            float maxNextQ = float.MinValue;
            for (int i = 0; i < _actions.Count; i++)
            {
                float nextQ = qLearningTable.GetQValue(currentStateKey, i);
                maxNextQ = Mathf.Max(maxNextQ, nextQ);
            }

            // Cập nhật Q-value theo công thức Q-learning
            float newQ = currentQ + learningRate * (reward + discountFactor * maxNextQ - currentQ);
            qLearningTable.UpdateQValue(lastStateKey, lastAction, newQ);
        }

        public void AddReward(float reward)
        {
            _reward += reward;
        }

        public void ResetReward()
        {
            _reward = 0;
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
                float qValue = qLearningTable.GetQValue(worldState.GetStateKey(), i);
                if (qValue > maxQ)
                {
                    maxQ = qValue;
                    bestAction = i;
                }
            }

            return bestAction;
        }
    }
}
