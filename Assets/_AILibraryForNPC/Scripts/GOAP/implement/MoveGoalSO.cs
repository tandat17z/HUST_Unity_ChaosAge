using System;
using System.Collections.Generic;
using System.Linq;
using AILibraryForNPC.core;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveGoalSO", menuName = "GOAP/Goal/MoveGoalSO")]
public class MoveGoalSO : BaseGoapGoalSO
{
    public override List<BaseActionSO> CreatePlan(WorldState state)
    {
        var planner = new GOAPPlanner(new MoveHeuricstic());
        var startState = state as MoveWorldState;
        var goalState = GetGoalState(state);
        var actions = availableActions.Cast<BaseGoapActionSO>().ToList();
        var plan = planner.Plan(startState, goalState, actions);
        return plan.Cast<BaseActionSO>().ToList();
    }

    private GoapWorldState GetGoalState(WorldState state)
    {
        var goalState = new MoveWorldState();
        var moveState = state as MoveWorldState;
        goalState.TargetX = moveState.TargetX;
        goalState.TargetY = moveState.TargetY;
        return goalState;
    }
}
