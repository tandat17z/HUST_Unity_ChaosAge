using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.core.Modules.GOAP.Actions
{
    public class GoToBuildingAction : GOAPAction
    {
        public GameObject target;
        private NavMeshAgent agent;
        private float stoppingDistance = 5f;

        protected override void InitializePreconditions()
        {
            preconditions.Add("hasTarget", 1);
        }

        protected override void InitializeEffects()
        {
            effects.Add("isAtTarget", 1);
        }

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
        }

        public override void Perform()
        {
            if (target == null)
            {
                isExecuting = false;
                return;
            }

            float distanceToTarget = Vector3.Distance(
                target.transform.position,
                transform.position
            );
            if (agent.hasPath && distanceToTarget <= stoppingDistance)
            {
                isExecuting = false;
            }
        }

        public override bool PrePerform()
        {
            if (target != null)
            {
                agent.SetDestination(target.transform.position);
                return true;
            }
            return false;
        }

        public override void PostPerform()
        {
            target = null;
        }

        public void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }

        public override void PrePerform(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }

        public override void Perform(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }

        public override void PostPerform(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsActionComplete(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }
    }
}
