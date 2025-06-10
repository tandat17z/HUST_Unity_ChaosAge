using System;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using ChaosAge.Battle;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPGoHome : GOAPAction
    {
        private NavMeshAgent _navMeshAgent;
        private GameObject _target;
        private float _playerHp;
        private float _range;

        public GOAPGoHome() {
            _playerHp = 100;
            _range = 2f;
        }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", (int)PlayerState.Home);

            state.AddState("PlayerHp", _playerHp);
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            _target = state.GetBuffer("Home") as GameObject;
            return state.GetState("PlayerHp") > 0
                && state.GetState("PlayerState") != (int)PlayerState.Home;
        }

        public override float GetCost()
        {
            return 2f;
        }

        public override bool IsComplete(WorldState_v2 worldState)
        {
            // Debug.Log("IsComplete go home");
            return _target == null
                || Vector3.Distance(agent.transform.position, _target.transform.position) < _range;
        }

        public override void Perform(WorldState_v2 worldState)
        {
            // Debug.Log("Update go home");
        }

        public override void PostPerform(WorldState_v2 worldState)
        {
            // Debug.LogWarning("PostPerform go home");
            _navMeshAgent.isStopped = true;
            _navMeshAgent.speed = 1;

            (agent as GOAPBarbarian).currentState = PlayerState.Idle;
            agent.GetComponent<BattleUnit>().AddHealth(_playerHp);
        }

        public override void PrePerform(WorldState_v2 worldState)
        {
            _navMeshAgent = agent.GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = 4;
            _target = worldState.GetBuffer("Home") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
            }
            (agent as GOAPBarbarian).currentState = PlayerState.Home;
            // agent.GetComponent<BattleUnit>().AddHealth(_playerHp);
        }
    }
}
