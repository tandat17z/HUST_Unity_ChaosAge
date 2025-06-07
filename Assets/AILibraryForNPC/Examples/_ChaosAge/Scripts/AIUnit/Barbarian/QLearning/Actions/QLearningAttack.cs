using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.QLearning;
using ChaosAge.data;
using UnityEngine;

namespace AILibraryForNPC.Examples
{
    public class QLearningAttack : QLearningAction
    {
        private BattleBuilding _target;
        private float _countTime;
        public float interval = 1;
        private string _previousStateKey;

        public override bool IsComplete(WorldState_v2 worldState)
        {
            return _countTime < 0 || _previousStateKey != worldState.GetStateKey();
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
            _previousStateKey = worldState.GetStateKey();
            _target = worldState.GetBuffer("target") as BattleBuilding;

            if (_target != null)
            {
                // trong pham vi tran cong
                if (Vector3.Distance(agent.transform.position, _target.transform.position) < 3f)
                {
                    _target.TakeDamage(10);
                    AddReward(10);

                    if (_target == null && _target.type == EBuildingType.TownHall)
                    {
                        AddReward(100);
                    }
                    if (_target == null && _target.type == EBuildingType.ArcherTowner)
                    {
                        AddReward(50);
                    }
                }
                else
                {
                    _target = null;
                    _countTime = -1;
                    AddReward(-10);
                }
            }
            else
            {
                _countTime = -1;
                AddReward(-10);
            }
        }
    }
}
