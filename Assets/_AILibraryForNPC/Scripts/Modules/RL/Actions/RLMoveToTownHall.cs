using AILibraryForNPC.core;
using AILibraryForNPC.Modules.RL;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTownHall : BaseAction
{
    string previousStateKey;
    NavMeshAgent agent;
    GameObject currentTarget;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    public override bool IsActionComplete(WorldState worldState)
    {
        return currentTarget == null
            || previousStateKey != (worldState as BarbarianWorldState).GetStateKey()
            || Vector3.Distance(transform.position, currentTarget.transform.position) < 3f;
    }

    public override void Perform(WorldState worldState) { }

    public override void PostPerform(WorldState worldState)
    {
        agent.isStopped = true;
    }

    public override void PrePerform(WorldState worldState)
    {
        previousStateKey = (worldState as BarbarianWorldState).GetStateKey();

        agent.isStopped = false;

        currentTarget = (worldState as BarbarianWorldState).targetTownhall;
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
            Debug.LogWarning("MoveToTownHall: " + currentTarget.name);

            (worldState as BarbarianWorldState).reward -= 1;
        }
    }
}
