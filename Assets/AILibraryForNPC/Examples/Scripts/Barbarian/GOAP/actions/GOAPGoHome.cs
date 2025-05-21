using System;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPGoHome : GOAPAction
    {
        private NavMeshAgent _navMeshAgent;
        private GameObject _target;

        public GOAPGoHome() { }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", (int)PlayerState.Home);

            state.AddState("PlayerHp", 100);
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            _target = state.GetBuffer("Home") as GameObject;
            return (
                    state.GetState("PlayerState") == (int)PlayerState.AttackTownhall
                    || state.GetState("PlayerState") == (int)PlayerState.AttackDefense
                )
                && state.GetState("PlayerHp") > 0;
        }

        public override float GetCost()
        {
            try
            {
                return Vector3.Distance(agent.transform.position, _target.transform.position);
            }
            catch (Exception e)
            {
                Debug.Log("Error getting cost: " + e.Message);
                return 1;
            }
        }

        public override bool IsComplete(WorldState_v2 worldState)
        {
            float range = 0.1f;
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
            _target = worldState.GetBuffer("Home") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
            }
        }
    }
}
