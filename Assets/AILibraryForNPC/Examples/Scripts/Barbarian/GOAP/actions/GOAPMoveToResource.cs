using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPMoveToDefense : GOAPAction
    {
        private NavMeshAgent _navMeshAgent;
        private GameObject _target;

        public GOAPMoveToDefense() { }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", 1);
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            _target = state.GetBuffer("Defense") as GameObject;
            return state.GetState("DefenseHp") > 0 && state.GetState("Moving") == 0;
        }

        public override float GetCost()
        {
            return Vector3.Distance(agent.transform.position, _target.transform.position);
        }

        public override bool IsComplete(WorldState_v2 worldState)
        {
            float range = 3f;
            return _target == null
                || Vector3.Distance(agent.transform.position, _target.transform.position) < range;
        }

        public override void Perform(WorldState_v2 worldState) { }

        public override void PostPerform(WorldState_v2 worldState)
        {
            _navMeshAgent.isStopped = true;
        }

        public override void PrePerform(WorldState_v2 worldState)
        {
            _navMeshAgent = agent.GetComponent<NavMeshAgent>();
            _target = worldState.GetBuffer("Defense") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
            }
        }
    }
}
