using System.Collections.Generic;
using System.Linq;
using AILibraryForNPC.Core;
using AILibraryForNPC.GOAP;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPGoalSystem : MonoBehaviour
    {
        public List<GOAPBaseGoal> goals = new List<GOAPBaseGoal>();
        private GOAPBaseGoal _currentGoal;
        private WorldState_v2 _worldState;

        public void AddGoal(GOAPBaseGoal goal)
        {
            goals.Add(goal);
        }

        public GOAPBaseGoal GetCurrentGoal(WorldState_v2 worldState)
        {
            if (goals == null || goals.Count == 0)
                return null;

            // Tính trọng số của từng goal dựa trên worldState
            _worldState = worldState;
            var weightedGoals = new List<(GOAPBaseGoal goal, float weight)>();
            foreach (var goal in goals)
            {
                float weight = goal.GetWeight(worldState);
                weightedGoals.Add((goal, weight));
            }

            // Tìm trọng số lớn nhất
            float maxWeight = weightedGoals.Max(g => g.weight);

            // Lọc các goal có trọng số lớn nhất
            var bestGoals = weightedGoals.Where(g => g.weight == maxWeight).ToList();

            // Chọn ngẫu nhiên nếu có nhiều goal bằng trọng số
            int index = Random.Range(0, bestGoals.Count);
            _currentGoal = bestGoals[index].goal;
            Debug.Log("GetCurrentGoal: " + _currentGoal.GetName() + " " + maxWeight);
            return _currentGoal;
        }

        public GOAPBaseGoal GetCurrentGoal()
        {
            return _currentGoal;
        }

        public WorldState_v2 GetWorldState()
        {
            return _worldState;
        }

        public int GetCurrentGoalIndex()
        {
            return goals.IndexOf(_currentGoal);
        }
    }
}
