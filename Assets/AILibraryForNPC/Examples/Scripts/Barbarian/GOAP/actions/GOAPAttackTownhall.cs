using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPAttackTownhall : GOAPAction
    {
        private NavMeshAgent _navMeshAgent;
        private GameObject _target;

        public GOAPAttackTownhall() { }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", (int)PlayerState.Attack);

            var townhallHp = state.GetState("TownhallHp");
            state.AddState("TownhallHp", townhallHp - 5);

            // neu khoang cach giua townhall va defense nho thì player bị trừ máu
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            return state.GetState("PlayerState") == (int)PlayerState.MoveToTownhall
                && state.GetState("TownhallHp") > 0
                && state.GetState("PlayerHp") > 0;
        }

        public override float GetCost()
        {
            return 1;
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
            _target = worldState.GetBuffer("targetTownhall") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
            }
        }
    }
}
