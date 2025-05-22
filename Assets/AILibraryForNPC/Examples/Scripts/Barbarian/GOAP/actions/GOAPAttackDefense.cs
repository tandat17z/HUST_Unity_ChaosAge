using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPAttackDefense : GOAPAction
    {
        private GameObject _target;
        private float _interval;

        public GOAPAttackDefense() { }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", (int)PlayerState.AttackDefense);

            var defenseHp = state.GetState("DefenseHp");
            state.AddState("DefenseHp", defenseHp - 5);

            var playerHp = state.GetState("PlayerHp");
            state.AddState("PlayerHp", playerHp - 6);
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            return (
                    state.GetState("PlayerState") == (int)PlayerState.MoveToDefense
                    || state.GetState("PlayerState") == (int)PlayerState.AttackDefense
                )
                && state.GetState("DefenseHp") > 0
                && state.GetState("PlayerHp") > 0;
        }

        public override float GetCost()
        {
            return 1;
        }

        public override bool IsComplete(WorldState_v2 worldState)
        {
            return _interval < 0;
        }

        public override void Perform(WorldState_v2 worldState)
        {
            _interval -= Time.deltaTime;
        }

        public override void PostPerform(WorldState_v2 worldState) { }

        public override void PrePerform(WorldState_v2 worldState)
        {
            _target = worldState.GetBuffer("Defense") as GameObject;
            _interval = 1;

            _target.GetComponent<BattleBuilding>().TakeDamage(5);
        }
    }
}
