using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;

namespace AILibraryForNPC.Examples
{
    public class GOAPAttack : GOAPAction
    {
        private Dictionary<string, float> _precondition;
        private Dictionary<string, float> _effect;
        private float _cost;

        private BattleBuilding _target;
        private float _countTime;
        public float interval = 1;

        public GOAPAttack()
        {
            _precondition = new Dictionary<string, float>();
            _effect = new Dictionary<string, float>();

            _precondition.Add("hasTarget", 1);
            _effect.Add("attack", 1);
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
            return _countTime < 0;
        }

        public override void Perform(WorldState_v2 worldState)
        {
            _countTime -= Time.deltaTime;
        }

        public override void PostPerform(WorldState_v2 worldState)
        {
            _countTime = interval;
        }

        public override void PrePerform(WorldState_v2 worldState)
        {
            _target = worldState.GetBuffer("target") as BattleBuilding;
            if (_target != null)
            {
                _target.TakeDamage(5);
            }
        }

        public override bool CheckPrecondition(Dictionary<string, float> state)
        {
            throw new System.NotImplementedException();
        }

        public override void ApplyEffect(Dictionary<string, float> state)
        {
            throw new System.NotImplementedException();
        }
    }
}
