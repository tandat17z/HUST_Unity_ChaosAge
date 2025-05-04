using System.Collections.Generic;
using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public class GOAPActionSystem : ActionSystem_v2
    {
        [SerializeField]
        private List<KeyValuePair<string, float>> listGoal;

        private GOAPPlanner _planner;
        private Queue<GOAPAction> _actionQueue;
        private Dictionary<string, float> _goal;

        protected override void OnInitialize()
        {
            _planner = new GOAPPlanner();

            foreach (var goal in listGoal)
            {
                _goal[goal.Key] = goal.Value;
            }
        }

        public override BaseAction_v2 SelectAction(WorldState_v2 worldState)
        {
            // Lên kế hoạch action mới nếu cần
            if (_actionQueue == null || _actionQueue.Count == 0)
            {
                var goapActions = _actions.ConvertAll(action => action as GOAPAction);
                _actionQueue = _planner.Plan(goapActions, _goal, worldState);
            }

            // Lấy action tiếp theo từ plan
            if (_actionQueue != null && _actionQueue.Count > 0)
            {
                var currentAction = _actionQueue.Dequeue();
                return currentAction;
            }
            return null;
        }
    }
}
