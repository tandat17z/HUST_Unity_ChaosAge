using System.Collections.Generic;
using System.Numerics;

public class MoveWorldState : GoapWorldState
{
    public int X;
    public int Y;

    public List<Vector2> buildingPositions;
    public int TargetX;
    public int TargetY;

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
