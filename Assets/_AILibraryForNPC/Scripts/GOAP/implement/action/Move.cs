using AILibraryForNPC.core;
using ChaosAge.manager;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "GOAP/Action/Move")]
public class Move : BaseGoapActionSO
{
    public int deltaX;
    public int deltaY;

    public float speed = 1f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float startTime;

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

    public override void StartExecute(Agent agent, WorldState worldState)
    {
        base.StartExecute(agent, worldState);
        var instanceState = worldState as MoveWorldState;
        var position = BattleVector2.GridToWorldPosition(
            new BattleVector2Int(instanceState.X + deltaX, instanceState.Y + deltaY)
        );
        var pos = BuildingManager.Instance.Grid.transform.TransformPoint(
            new Vector3(position.x, 0, position.y)
        );
        startPosition = agent.transform.position;
        endPosition = pos;
        startTime = Time.time;
    }

    public override void ExecutePerFrame(Agent agent)
    {
        var t = (Time.time - startTime) / speed;
        agent.transform.position = Vector3.Lerp(startPosition, endPosition, t);

        if (Vector3.Distance(agent.transform.position, endPosition) < 0.01f)
        {
            StopExecute(agent);
        }
    }
}
