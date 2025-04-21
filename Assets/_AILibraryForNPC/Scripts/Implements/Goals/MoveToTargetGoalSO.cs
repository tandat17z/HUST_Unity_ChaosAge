using System.Collections.Generic;
using AILibraryForNPC.core;
using AILibraryForNPC.core.Modules.Pathfinding;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToTargetGoal", menuName = "AI/Goals/MoveToTarget")]
public class MoveToTargetGoalSO : BaseGoalSO
{
    private GridNode startNode;
    private GridNode targetNode;
    private AStarPathfinder<GridNode> pathfinder;

    private void OnEnable()
    {
        pathfinder = new AStarPathfinder<GridNode>();
    }

    public override List<BaseActionSO> CreatePlan(WorldState state)
    {
        var gridGraph = state.GetState("gridGraph") as GridGraph;
        List<BaseActionSO> plan = new List<BaseActionSO>();

        if (!state.ContainsKey("targetPosition") || !state.ContainsKey("currentPosition"))
            return plan;


        if (startNode == null || targetNode == null)
            return plan;

        var path = pathfinder.FindPath(startNode, targetNode, gridGraph);

        if (path == null || path.Count <= 1)
            return plan;

        // Chuyển đổi path thành các action
        for (int i = 1; i < path.Count; i++)
        {
            Vector2Int direction = path[i].GridPosition - path[i - 1].GridPosition;
            BaseActionSO action = GetActionForDirection(direction);
            if (action != null)
            {
                plan.Add(action);
            }
        }

        return plan;
    }

    private BaseActionSO GetActionForDirection(Vector2Int direction)
    {
        foreach (var action in availableActions)
        {
            if (action is MoveActionSO && direction == Vector2Int.left)
                return action;
            // if (action is MoveRightActionSO && direction == Vector2Int.right)
            //     return action;
            // if (action is MoveUpActionSO && direction == Vector2Int.up)
            //     return action;
            // if (action is MoveDownActionSO && direction == Vector2Int.down)
            //     return action;
        }
        return null;
    }
}
