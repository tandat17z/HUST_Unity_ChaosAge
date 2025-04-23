using AILibraryForNPC.core;
using ChaosAge.manager;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "GOAP/Action/Move")]
public class Move : BaseGoapActionSO
{
    public int deltaX;
    public int deltaY;

    public float speed = 1f;

    public override GoapWorldState ApplyEffects(GoapWorldState state)
    {
        var instanceState = state as MoveWorldState;
        var newState = new MoveWorldState();
        newState.X = instanceState.X + deltaX;
        newState.Y = instanceState.Y + deltaY;
        return newState;
    }

    public override bool ArePreconditionsSatisfied(GoapWorldState state)
    {
        var instanceState = state as MoveWorldState;
        return instanceState.X + deltaX < 40
            && instanceState.Y + deltaY < 40
            && instanceState.X + deltaX >= 0
            && instanceState.Y + deltaY >= 0;
    }

    public override float GetCost(GoapWorldState state)
    {
        return 1;
    }

    public override void StartExecute(Agent agent, AILibraryForNPC.core.WorldState worldState)
    {
        base.StartExecute(agent, worldState);
        // var instanceState = worldState as MoveWorldState;
        // instanceState.startPosition = agent.transform.position;
        // var position = BattleVector2.GridToWorldPosition(
        //     new BattleVector2Int(instanceState.X + deltaX, instanceState.Y + deltaY)
        // );
        // var pos = BuildingManager.Instance.Grid.transform.TransformPoint(
        //     new Vector3(position.x, 0, position.y)
        // );
        // instanceState.endPosition = pos;
        // instanceState.startTime = Time.time;
        // worldState = instanceState;
    }

    public override void ExecutePerFrame(Agent agent, AILibraryForNPC.core.WorldState worldState)
    {
        // var instanceState = worldState as MoveWorldState;
        // agent.transform.position = Vector3.Lerp(
        //     instanceState.startPosition,
        //     instanceState.endPosition,
        //     (Time.time - instanceState.startTime) / speed
        // );
        // if (Vector3.Distance(agent.transform.position, instanceState.endPosition) < 0.01f)
        // {
        //     StopExecute(agent);
        // }
    }
}
