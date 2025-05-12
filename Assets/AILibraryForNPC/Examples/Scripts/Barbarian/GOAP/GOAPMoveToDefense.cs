using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPMoveToDefense : GOAPAction
    {
        private Dictionary<string, float> _precondition;
        private Dictionary<string, float> _effect;
        private float _cost;

        private NavMeshAgent _navMeshAgent;
        private GameObject _target;

        public GOAPMoveToDefense()
        {
            _precondition = new Dictionary<string, float>();
            _effect = new Dictionary<string, float>();
            _precondition.Add("hasDefense", 1);
            _effect.Add("hasTarget", 1);
        }

        public override void ApplyEffect(WorldState_v2 state)
        {
            throw new System.NotImplementedException();
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            throw new System.NotImplementedException();
        }

        public override float GetCost()
        {
            return 1;
        }

        public override Dictionary<string, float> GetEffect()
        {
            return _effect;
        }

        public override Dictionary<string, float> GetPrecondition()
        {
            return _precondition;
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
            _target = worldState.GetBuffer("targetDefense") as GameObject;
            if (_target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
            }
        }
    }
}
