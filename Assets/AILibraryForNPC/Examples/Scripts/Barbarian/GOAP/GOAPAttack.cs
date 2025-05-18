using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;

namespace AILibraryForNPC.Examples
{
    public class GOAPAttack : GOAPAction
    {
        private float _cost;

        private BattleBuilding _target;
        private float _countTime;
        public float interval = 1;

        public GOAPAttack()
        {
        }

        public override float GetCost()
        {
            return 1;
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

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            throw new System.NotImplementedException();
        }

        public override void ApplyEffect(WorldState_v2 state)
        {
            throw new System.NotImplementedException();
        }
    }
}
