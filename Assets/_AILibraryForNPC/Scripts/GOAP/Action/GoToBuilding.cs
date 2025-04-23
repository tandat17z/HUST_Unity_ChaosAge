using GOAPSystem;
using UnityEngine;
using UnityEngine.AI;

public class GoToHospital : GAction
{
    public GameObject target;
    public NavMeshAgent agent;

    public override bool IsActionComplete()
    {
        if (target == null)
            return true;

        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

        return agent.hasPath && distanceToTarget < 5f;
    }

    public override void Perform()
    {
        Debug.Log("Go to hospital");
    }

    public override void PostPerform()
    {
        target = null;
    }

    public override bool PrePerform()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
        return target != null;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    protected override void OnAwake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }
}
