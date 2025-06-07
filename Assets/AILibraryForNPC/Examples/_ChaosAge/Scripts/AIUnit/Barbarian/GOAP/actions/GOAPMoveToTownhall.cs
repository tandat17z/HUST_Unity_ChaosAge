using System;
using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPMoveToTownhall : GOAPAction
    {
        private NavMeshAgent _navMeshAgent;
        private GameObject _target;

        public GOAPMoveToTownhall() { }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", (int)PlayerState.MoveToTownhall);
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            _target = state.GetBuffer("Townhall") as GameObject;
            var playerState = state.GetState("PlayerState");
            return state.GetState("PlayerHp") > 0
                && state.GetState("TownhallHp") > 0
                && (
                    playerState != (int)PlayerState.AttackTownhall
                    && playerState != (int)PlayerState.MoveToTownhall
                );
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
                return 10;
            }
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
            _target = worldState.GetBuffer("Townhall") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
            }
        }
    }
}
