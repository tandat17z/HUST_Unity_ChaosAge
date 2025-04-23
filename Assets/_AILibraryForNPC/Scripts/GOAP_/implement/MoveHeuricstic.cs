using UnityEngine;

public class MoveHeuricstic : IHeuristic
{
    public float Heuristic(GoapWorldState from, GoapWorldState to)
    {
        var fromState = from as MoveWorldState;
        var toState = to as MoveWorldState;
        return Mathf.Pow(fromState.X - toState.TargetX, 2)
            + Mathf.Pow(fromState.Y - toState.TargetY, 2);
    }
}
