using System.Collections.Generic;
using UnityEngine;
using AILibraryForNPC.core;
using System.Linq;

namespace AILibraryForNPC.core.Implements.Goals
{
    [CreateAssetMenu(fileName = "GOAPGoal", menuName = "AI/Goals/GOAP")]
    public class GOAPGoalSO : BaseGoalSO
    {
        //    [SerializeField] private List<BaseActionSO> availableActions = new List<BaseActionSO>();
        //    [SerializeField] private Dictionary<string, object> goalState = new Dictionary<string, object>();

        //    public override List<BaseActionSO> CreatePlan(WorldState state)
        //    {
        //        // 1. Khởi tạo các node cho thuật toán A*
        //        var start = new GOAPNode
        //        {
        //            state = state.GetState(),
        //            action = null,
        //            parent = null,
        //            runningCost = 0
        //        };

        //        var goal = new GOAPNode
        //        {
        //            state = goalState,
        //            action = null,
        //            parent = null,
        //            runningCost = 0
        //        };

        //        // 2. Tìm đường đi từ start đến goal
        //        var path = FindPath(start, goal);

        //        // 3. Chuyển đổi path thành chuỗi action
        //        return BuildActionList(path);
        //    }

        //    private List<BaseActionSO> BuildActionList(GOAPNode endNode)
        //    {
        //        var result = new List<BaseActionSO>();
        //        var node = endNode;

        //        while (node != null && node.action != null)
        //        {
        //            result.Insert(0, node.action);
        //            node = node.parent;
        //        }

        //        return result;
        //    }

        //    private List<GOAPNode> FindPath(GOAPNode start, GOAPNode goal)
        //    {
        //        var openSet = new List<GOAPNode> { start };
        //        var closedSet = new HashSet<GOAPNode>();

        //        while (openSet.Count > 0)
        //        {
        //            // Lấy node có chi phí thấp nhất
        //            var current = openSet.OrderBy(n => n.runningCost + Heuristic(n.state, goal.state)).First();

        //            // Kiểm tra xem đã đạt được goal chưa
        //            if (IsGoalAchieved(current.state, goal.state))
        //            {
        //                return BuildPath(current);
        //            }

        //            openSet.Remove(current);
        //            closedSet.Add(current);

        //            // Tìm các node kế tiếp
        //            foreach (var action in availableActions)
        //            {
        //                if (action.CanExecute(current.state))
        //                {
        //                    var nextState = action.Execute(current.state);
        //                    var nextNode = new GOAPNode
        //                    {
        //                        state = nextState,
        //                        action = action,
        //                        parent = current,
        //                        runningCost = current.runningCost + CalculateActionCost(action, current.state)
        //                    };

        //                    if (!closedSet.Contains(nextNode) && !openSet.Contains(nextNode))
        //                    {
        //                        openSet.Add(nextNode);
        //                    }
        //                }
        //            }
        //        }

        //        return null; // Không tìm thấy đường đi
        //    }

        //    private float Heuristic(Dictionary<string, object> current, Dictionary<string, object> goal)
        //    {
        //        float cost = 0;
        //        foreach (var kvp in goal)
        //        {
        //            if (current.ContainsKey(kvp.Key))
        //            {
        //                if (!current[kvp.Key].Equals(kvp.Value))
        //                {
        //                    cost += 1;
        //                }
        //            }
        //            else
        //            {
        //                cost += 1;
        //            }
        //        }
        //        return cost;
        //    }

        //    private bool IsGoalAchieved(Dictionary<string, object> current, Dictionary<string, object> goal)
        //    {
        //        foreach (var kvp in goal)
        //        {
        //            if (!current.ContainsKey(kvp.Key) || !current[kvp.Key].Equals(kvp.Value))
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }

        //    private List<GOAPNode> BuildPath(GOAPNode endNode)
        //    {
        //        var path = new List<GOAPNode>();
        //        var current = endNode;

        //        while (current != null)
        //        {
        //            path.Insert(0, current);
        //            current = current.parent;
        //        }

        //        return path;
        //    }

        //    // Đánh giá mức độ phù hợp của goal
        //    public override float EvaluateGoal(WorldState state)
        //    {
        //        float score = 0;
        //        var currentState = state.GetState();

        //        foreach (var kvp in goalState)
        //        {
        //            if (currentState.ContainsKey(kvp.Key) && currentState[kvp.Key].Equals(kvp.Value))
        //            {
        //                score += 1;
        //            }
        //        }

        //        return score / goalState.Count;
        //    }

        //    // Kiểm tra tính khả thi của goal
        //    public override bool IsValid(WorldState state)
        //    {
        //        return availableActions.Count > 0 && goalState.Count > 0;
        //    }
        //}

        //public class GOAPNode
        //{
        //    public Dictionary<string, object> state;
        //    public BaseActionSO action;
        //    public GOAPNode parent;
        //    public float runningCost;

        //    public override bool Equals(object obj)
        //    {
        //        if (obj is GOAPNode other)
        //        {
        //            return state.SequenceEqual(other.state);
        //        }
        //        return false;
        //    }

        //    public override int GetHashCode()
        //    {
        //        return state.GetHashCode();
        //    }
        public override List<BaseActionSO> CreatePlan(WorldState state)
        {
            throw new System.NotImplementedException();
        }
    }
}
