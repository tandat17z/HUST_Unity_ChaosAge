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

        public void AddGoal(GOAPBaseGoal goal)
        {
            goals.Add(goal);
        }

        public GOAPBaseGoal GetCurrentGoal(WorldState_v2 worldState)
        {
            Debug.Log("GetCurrentGoal" + goals.Count);
            if (goals == null || goals.Count == 0)
                return null;

            // Tính trọng số của từng goal dựa trên worldState
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
            return _currentGoal;
        }

        public GOAPBaseGoal GetCurrentGoal()
        {
            return _currentGoal;
        }
    }
}
