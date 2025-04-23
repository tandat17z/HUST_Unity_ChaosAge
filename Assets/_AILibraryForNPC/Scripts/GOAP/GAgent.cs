using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AILibraryForNPC.GOAP
{
    [Serializable]
    public class SubGoal
    {
        public WorldState[] beliefs;
        public bool remove;
        private Dictionary<string, int> sgoals;

        public void Initialize()
        {
            sgoals = new Dictionary<string, int>();
            foreach (var belief in beliefs)
            {
                sgoals.Add(belief.key, belief.value);
            }
        }

        public SubGoal(string key, int value, bool remove)
        {
            sgoals = new Dictionary<string, int>();
            sgoals.Add(key, value);
            this.remove = remove;
        }

        public Dictionary<string, int> GetSGoals()
        {
            return sgoals;
        }
    }

    [Serializable] // Bắt buộc để Unity serialize được
    public class GoalEntry
    {
        public SubGoal key; // SubGoal là key
        public int value; // int là value
    }

    public class GAgent : MonoBehaviour
    {
        [SerializeField]
        private List<GAction> actions = new List<GAction>();

        [SerializeField]
        private List<GoalEntry> listGoalEntry = new List<GoalEntry>();
        protected Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

        GPlanner planner;
        Queue<GAction> actionQueue;

        [SerializeField, ReadOnly]
        private GAction currentAction;

        [SerializeField, ReadOnly]
        private SubGoal currentGoal;

        protected virtual void Start()
        {
            GAction[] acts = this.GetComponents<GAction>();
            foreach (GAction a in acts)
            {
                actions.Add(a);
            }

            foreach (var goal in listGoalEntry)
            {
                goals.Add(goal.key, goal.value);
                goal.key.Initialize();
            }
        }

        void LateUpdate()
        {
            // Check if the current action is running
            if (currentAction != null)
            {
                if (!currentAction.IsActionComplete())
                {
                    currentAction.Perform();
                    return;
                }
                else
                {
                    currentAction.PostPerform();
                    currentAction = null;
                }
            }

            // Lên kế hoạch action mới
            if (planner == null || actionQueue == null)
            {
                planner = new GPlanner();
                var sortedGoals = goals.OrderByDescending(x => x.Value);
                foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
                {
                    actionQueue = planner.plan(actions, sg.Key.GetSGoals(), null);
                    if (actionQueue != null)
                    {
                        currentGoal = sg.Key;
                        break;
                    }
                }
            }

            // Khi thực hiện hết plan? Có thể xóa subgoal không?
            if (actionQueue != null && actionQueue.Count == 0)
            {
                if (currentGoal.remove)
                {
                    goals.Remove(currentGoal);
                }
                planner = null;
            }

            // Lấy action tiếp theo từ plan
            if (actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                currentAction.PrePerform();
            }
        }
    }
}
