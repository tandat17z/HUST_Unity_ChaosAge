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
        private List<GOAPAction> _beginPlan = new();
        private WorldState_v2 _beginWorldState;
        private int _currentActionIndex;
        private float _delaySearch = 0;

        protected override void OnInitialize()
        {
            _planner = new GOAPPlanner();
            _planner.SetAlgorithm(_pathfindingAlgorithm);
            _agent = GetComponent<GOAPAgent>();
        }

        public override BaseAction_v2 SelectAction(WorldState_v2 worldState)
        {
            _currentActionIndex += 1;
            // Lên kế hoạch action mới nếu cần
            if (_actionQueue == null || _actionQueue.Count == 0)
            {
                var goapActions = new List<GOAPAction>();
                foreach (var action in _actions)
                {
                    goapActions.Add(action as GOAPAction);
                }

                if(_delaySearch > 0){
                    _delaySearch -= Time.deltaTime;
                    return null;
                }

                var bestGoal = _agent.goalSystem.GetCurrentGoal(worldState);
                _actionQueue = _planner.Plan(goapActions, bestGoal, worldState);

                if(_actionQueue != null){
                    _beginWorldState = worldState.Clone();
                    _beginPlan = new List<GOAPAction>(_actionQueue);
                    _currentActionIndex = 0;
                    _delaySearch = 0;
                }
                else{
                    _delaySearch = 5f;
                }
            }

            // Lấy action tiếp theo từ plan
            if (_actionQueue != null && _actionQueue.Count > 0)
            {
                var action = _actionQueue.Dequeue();
                return action;
            }
            return null;
        }

        public void CancelPlan()
        {
            _actionQueue = new Queue<GOAPAction>();
        }

        public List<GOAPAction> GetCurrentPlan()
        {
            return _beginPlan;
        }

        public int GetCurrentActionIndex()
        {
            return _currentActionIndex;
        }

        public List<GOAPAction> GetActions()
        {
            var actions = new List<GOAPAction>();
            foreach (var action in _actions)
            {
                actions.Add(action as GOAPAction);
            }
            return actions;
        }

        public WorldState_v2 GetBeginWorldState()
        {
            return _beginWorldState;
        }
    }
}
