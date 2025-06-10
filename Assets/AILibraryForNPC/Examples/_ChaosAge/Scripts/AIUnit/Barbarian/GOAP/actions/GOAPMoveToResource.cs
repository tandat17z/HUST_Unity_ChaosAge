using System;
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
        private float _range;

        public GOAPMoveToDefense() {
            _range = 3f;
        }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", (int)PlayerState.MoveToDefense);
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            _target = state.GetBuffer("Defense") as GameObject;
            var playerState = state.GetState("PlayerState");
            return state.GetState("PlayerHp") > 0
                && state.GetState("DefenseHp") > 0
                && (
                    playerState != (int)PlayerState.AttackDefense
                    && playerState != (int)PlayerState.MoveToDefense
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
            return _target == null
                || Vector3.Distance(agent.transform.position, _target.transform.position) < _range;
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
