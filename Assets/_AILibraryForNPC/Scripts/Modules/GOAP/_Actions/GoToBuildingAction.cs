using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.core.Modules.GOAP.Actions
{
    public class GoToBuildingAction : GOAPAction
    {
        [SerializeField, ReadOnly]
        private NavMeshAgent agent;

        [SerializeField, ReadOnly]
        private GameObject target;

        [SerializeField]
        private float range = 5f;

        protected override void OnAwake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
        }

        public override bool IsActionComplete(WorldState worldState)
        {
            if (target == null)
                return true;

            float distanceToTarget = Vector3.Distance(
                target.transform.position,
                transform.position
            );

            return agent.hasPath && distanceToTarget < range;
        }

        public override void Perform(WorldState worldState) // update by frame
        {
            // Debug.Log("Go to hospital");
        }

        public override void PostPerform(WorldState worldState)
        {
            foreach (var effect in effects)
            {
                worldState.ModifyState(effect.Key, effect.Value);
            }
            agent.isStopped = true;
            target = null;
        }

        public override void PrePerform(WorldState worldState)
        {
            var findBuildingSensor = worldState.GetSensor<FindBuildingSensor>();
            if (findBuildingSensor != null)
            {
                target = findBuildingSensor.targetBuilding.gameObject;
            }
            if (target != null)
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }
        }
    }
}
