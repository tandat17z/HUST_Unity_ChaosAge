using System.Collections.Generic;
using UnityEngine;

public class MoveWorldState : GoapWorldState
{
    public int X;
    public int Y;
    public int TargetX;
    public int TargetY;

    public Vector3 TargetPosition;
    internal Vector3 startPosition;
    internal Vector3 endPosition;
    internal float startTime;

    public override bool IsSatisfiedBy(GoapWorldState goalState)
    {
        var goal = goalState as MoveWorldState;
        return X == goal.TargetX && Y == goal.TargetY;
    }

    public override int CustomEquals(GoapWorldState other)
    {
        var state = other as MoveWorldState;
        return X == state.X && Y == state.Y ? 1 : 0;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() + Y.GetHashCode();
    }
}
