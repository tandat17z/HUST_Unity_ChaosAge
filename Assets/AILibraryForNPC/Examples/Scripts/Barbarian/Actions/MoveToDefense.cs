using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.QLearning;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class MoveToDefense : QLearningAction
    {
        private NavMeshAgent _navMeshAgent;
        private GameObject _target;
        private string _previousStateKey;

        public override bool IsComplete(WorldState_v2 worldState)
        {
            float range = 3f;
            return _target == null
                || Vector3.Distance(agent.transform.position, _target.transform.position) < range
                || _previousStateKey != worldState.GetStateKey();
        }

        public override void Perform(WorldState_v2 worldState) { }

        public override void PostPerform(WorldState_v2 worldState)
        {
            _navMeshAgent.isStopped = true;
        }

        public override void PrePerform(WorldState_v2 worldState)
        {
            _navMeshAgent = agent.GetComponent<NavMeshAgent>();

            _target = worldState.GetBuffer("targetDefense") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.SetDestination(_target.transform.position);
            }
        }
    }
}
