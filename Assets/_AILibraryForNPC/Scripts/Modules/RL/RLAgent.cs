using System.Collections.Generic;
using System.IO;
using AILibraryForNPC.core;
using Newtonsoft.Json;
using UnityEngine;

namespace AILibraryForNPC.Modules.RL
{
    [RequireComponent(typeof(RLPerceptionSystem))]
    public class RLAgent : Agent
    {
        [SerializeField]
        private bool isTraining = true;

        [SerializeField]
        private int checkpointIndex = 1;

        public static Dictionary<string, float> qTable = new Dictionary<string, float>();
        private float learningRate = 0.1f;
        private float discountFactor = 0.9f;

        [SerializeField]
        private float explorationRate = 0.1f;

        private string lastState;
        private int lastAction;

        public override void Initialize()
        {
            qTable = new Dictionary<string, float>();
            lastState = null;
            lastAction = -1;
            LoadModel(checkpointIndex);
        }

        protected override void UpdateDecisionMaking(WorldState worldState)
        {
            // Thực hiện action
            if (currentAction != null)
            {
                if (!currentAction.IsActionComplete(worldState)) // Khi thay đổi state thì coi như kết thúc action
                {
                    currentAction.Perform(worldState);
                    return;
                }
                else
                {
                    currentAction.PostPerform(worldState);
                    currentAction = null;
                }
            }

            // Cập nhật Q-table nếu có state và action trước đó
            // Chuyển đổi WorldState thành state key cho Q-learning
            string currentState = (worldState as RLWorldState)?.GetStateKey();
            int action = ChooseAction(worldState);

            // Chọn action dựa trên Q-learning
            if (isTraining && lastState != null && lastAction != -1)
            {
                float reward = CalculateReward(worldState);
                UpdateQValue(lastState, lastAction, reward, currentState);
            }

            // Lưu state và action hiện tại
            lastState = currentState;
            lastAction = action;

            // Thực hiện action mới
            currentAction = actionSystem?.GetAvailableActions()[action];
            if (currentAction != null)
            {
                currentAction.PrePerform(worldState);
            }
            SaveModel(checkpointIndex);
        }

        private int ChooseAction(WorldState worldState)
        {
            // Exploration
            if (isTraining && Random.value < explorationRate)
            {
                return Random.Range(0, actionSystem.GetAvailableActions().Count);
            }

            // Exploitation
            float maxQ = float.MinValue;
            int bestAction = 0;

            var availableActions = actionSystem.GetAvailableActions();
            var state = (worldState as RLWorldState)?.GetStateKey();
            for (int i = 0; i < availableActions.Count; i++)
            {
                float qValue = GetQValue(state, i);
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

        private void UpdateQValue(string state, int action, float reward, string nextState)
        {
            string key = $"{state}_{action}";
            float currentQ = GetQValue(state, action);

            // Tìm Q-value lớn nhất cho next state
            float maxNextQ = float.MinValue;
            var availableActions = actionSystem.GetAvailableActions();
            for (int i = 0; i < availableActions.Count; i++)
            {
                float nextQ = GetQValue(nextState, i);
                maxNextQ = Mathf.Max(maxNextQ, nextQ);
            }

            // Cập nhật Q-value theo công thức Q-learning
            float newQ = currentQ + learningRate * (reward + discountFactor * maxNextQ - currentQ);
            qTable[key] = newQ;
        }

        private float CalculateReward(WorldState worldState)
        {
            float reward = (worldState as BarbarianWorldState).GetReward();
            Debug.LogWarning("CalculateReward: " + reward);
            return reward;
        }

        public void SaveModel(int checkpointIndex)
        {
            string json = JsonConvert.SerializeObject(qTable, Formatting.Indented);
            string path = Application.dataPath + "/Models/" + checkpointIndex + ".json";
            File.WriteAllText(path, json);
        }

        public void LoadModel(int checkpointIndex)
        {
            string path = Application.dataPath + "/Models/" + checkpointIndex + ".json";
            string json = File.ReadAllText(path);
            qTable = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);
        }
    }
}
