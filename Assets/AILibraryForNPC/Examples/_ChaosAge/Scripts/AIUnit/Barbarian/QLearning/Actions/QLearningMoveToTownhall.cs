using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.QLearning;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class QLearningMoveToTownhall : QLearningAction
    {
        private NavMeshAgent _navMeshAgent;
        private GameObject _target;
        private string _previousStateKey;

        public override bool IsComplete(WorldState_v2 worldState)
        {
            float range = 3f;

            if (
                _target == null
                || Vector3.Distance(agent.transform.position, _target.transform.position) < range
                || _previousStateKey != worldState.GetStateKey()
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Perform(WorldState_v2 worldState) { }

        public override void PostPerform(WorldState_v2 worldState)
        {
            _navMeshAgent.isStopped = true;
        }

        public override void PrePerform(WorldState_v2 worldState)
        {
            _previousStateKey = worldState.GetStateKey();
            _navMeshAgent = agent.GetComponent<NavMeshAgent>();

            _target = worldState.GetBuffer("targetTownhall") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);

                if (Vector3.Distance(agent.transform.position, _target.transform.position) < 3f)
                {
                    AddReward(-10);
                }
                else
                {
                    AddReward(-3); // chi phí di chuyển
                }
            }
        }
    }
}
