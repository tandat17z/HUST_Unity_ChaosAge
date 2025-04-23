using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAPPlanner
{
    // private IHeuristic heuristic;

    // private class Node
    // {
    //     public GoapWorldState State;
    //     public Node Parent;
    //     public BaseGoapActionSO Action;
    //     public float G; // Cost so far
    //     public float H; // Heuristic to goal
    //     public float F => G + H;
    // }

    // public GOAPPlanner(IHeuristic heuristic)
    // {
    //     this.heuristic = heuristic;
    // }

    // public List<BaseGoapActionSO> Plan(
    //     GoapWorldState startState,
    //     GoapWorldState goalState,
    //     List<BaseGoapActionSO> actions
    // )
    // {
    //     var moveState = startState as MoveWorldState;
    //     var goalMoveState = goalState as MoveWorldState;
    //     Debug.Log(
    //         "GOAPPlanner Plan "
    //             + moveState.X
    //             + " "
    //             + moveState.Y
    //             + " "
    //             + goalMoveState.TargetX
    //             + " "
    //             + goalMoveState.TargetY
    //     );
    //     var openSet = new List<Node>();
    //     var closedSet = new List<GoapWorldState>();

    //     Node startNode = new Node
    //     {
    //         State = startState,
    //         G = 0,
    //         H = Heuristic(startState, goalState),
    //     };
    //     openSet.Add(startNode);

    //     var count = 0;
    //     while (openSet.Count > 0)
    //     {
    //         count += 1;
    //         if (count > 1000)
    //             break;
    //         // Chọn node có F thấp nhất
    //         Node current = openSet.OrderBy(n => n.F).First();
    //         Debug.Log(
    //             "GOAPPlanner Plan current "
    //                 + (current.State as MoveWorldState).X
    //                 + " "
    //                 + (current.State as MoveWorldState).Y
    //         );
    //         openSet.Remove(current);

    //         if (current.State.IsSatisfiedBy(goalState))
    //         {
    //             Debug.LogWarning("GOAPPlanner Plan satisfied");
    //             return ReconstructPlan(current);
    //         }

    //         closedSet.Add(current.State);

    //         foreach (var action in actions)
    //         {
    //             if (!action.ArePreconditionsSatisfied(current.State))
    //             {
    //                 continue;
    //             }

    //             GoapWorldState nextState = action.ApplyEffects(current.State);
    //             if (closedSet.FindIndex(s => s.CustomEquals(nextState) == 1) != -1)
    //             {
    //                 continue;
    //             }
    //             Node nextNode = new Node
    //             {
    //                 State = nextState,
    //                 Parent = current,
    //                 Action = action,
    //                 G = current.G + action.GetCost(current.State),
    //                 H = Heuristic(nextState, goalState),
    //             };

    //             openSet.Add(nextNode);
    //         }
    //     }

    //     return null; // No path found
    // }

    // private List<BaseGoapActionSO> ReconstructPlan(Node node)
    // {
    //     var actions = new List<BaseGoapActionSO>();
    //     while (node.Parent != null)
    //     {
    //         actions.Add(node.Action);
    //         node = node.Parent;
    //     }
    //     actions.Reverse();
    //     return actions;
    // }

    // private float Heuristic(GoapWorldState from, GoapWorldState to)
    // {
    //     return heuristic.Heuristic(from, to);
    // }
}
