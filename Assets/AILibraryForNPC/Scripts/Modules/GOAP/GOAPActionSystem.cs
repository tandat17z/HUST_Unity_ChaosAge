using System;
using System.Collections.Generic;
using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPActionSystem : ActionSystem_v2
    {
        [SerializeField]
        private PathfindingAlgorithm _pathfindingAlgorithm;

        private GOAPAgent _agent;
        private GOAPPlanner _planner;
        private Queue<GOAPAction> _actionQueue = new();

        protected override void OnInitialize()
        {
            _planner = new GOAPPlanner();
            _planner.SetAlgorithm(_pathfindingAlgorithm);
            _agent = GetComponent<GOAPAgent>();
        }

        public override BaseAction_v2 SelectAction(WorldState_v2 worldState)
        {
            // Lên kế hoạch action mới nếu cần
            if (_actionQueue == null || _actionQueue.Count == 0)
            {
                var goapActions = new List<GOAPAction>();
                foreach (var action in _actions)
                {
                    goapActions.Add(action as GOAPAction);
                }

                var bestGoal = _agent.goalSystem.GetCurrentGoal(worldState);
                Debug.Log("Best goal: " + bestGoal.GetName());
                // Debug.Log("Count actions: " + goapActions.Count);
                _actionQueue = _planner.Plan(goapActions, bestGoal, worldState);
            }

            // Lấy action tiếp theo từ plan
            if (_actionQueue != null && _actionQueue.Count > 0)
            {
                var currentAction = _actionQueue.Dequeue();
                return currentAction;
            }
            return null;
        }

        public void CancelPlan()
        {
            _actionQueue = new Queue<GOAPAction>();
        }
    }
}
