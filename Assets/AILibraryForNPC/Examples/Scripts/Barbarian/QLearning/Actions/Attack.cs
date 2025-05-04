using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.QLearning;
using ChaosAge.Config;
using UnityEngine;

namespace AILibraryForNPC.Examples
{
    public class Attack : QLearningAction
    {
        private BattleBuilding _target;
        private float _countTime;
        public float interval = 1;

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
                AddReward(5);

                if (_target == null && _target.type == EBuildingType.townhall)
                {
                    AddReward(100);
                }
                if (_target == null && _target.type == EBuildingType.archertower)
                {
                    AddReward(50);
                }
            }
            else
            {
                AddReward(-3);
            }
        }
    }
}
