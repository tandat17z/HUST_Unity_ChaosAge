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
        public Dictionary<string, int> sgoals;
        public bool remove;

        public SubGoal(string key, int value, bool remove)
        {
            sgoals = new Dictionary<string, int>();
            sgoals.Add(key, value);
            this.remove = remove;
        }
    }

    public class GAgent : MonoBehaviour
    {
        [SerializeField]
        private List<GAction> actions = new List<GAction>();

        [SerializeField]
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
        }

        bool invoked = false;

        void CompleteAction()
        {
            currentAction.running = false;
            currentAction.PostPerform();
            invoked = false;
        }

        void LateUpdate()
        {
            // Check if the current action is running
            if (currentAction != null && currentAction.running)
            {
                float distanceToTarget = Vector3.Distance(
                    currentAction.target.transform.position,
                    transform.position
                );
                if (currentAction.agent.hasPath && distanceToTarget < 2f)
                {
                    if (!invoked)
                    {
                        Invoke(nameof(CompleteAction), currentAction.duration);
                        invoked = true;
                    }
                }
                return;
            }

            // Lên kế hoạch action mới
            if (planner == null || actionQueue == null)
            {
                planner = new GPlanner();
                var sortedGoals = goals.OrderByDescending(x => x.Value);
                foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
                {
                    actionQueue = planner.plan(actions, sg.Key.sgoals, null);
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
                if (currentAction.PrePerform())
                {
                    // Tìm đích mới
                    if (currentAction.target == null && currentAction.targetTag != "")
                    {
                        currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                    }

                    // Chạy đích mới
                    if (currentAction.target != null)
                    {
                        currentAction.running = true;
                        currentAction.agent.SetDestination(currentAction.target.transform.position);
                    }
                }
                else
                {
                    actionQueue = null;
                }
            }
        }
    }
}
