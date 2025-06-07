using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPAttackTownhall : GOAPAction
    {
        private GameObject _target;
        private float _interval;

        public GOAPAttackTownhall() { }

        public override void ApplyEffect(WorldState_v2 state)
        {
            state.AddState("PlayerState", (int)PlayerState.AttackTownhall);

            var townhallHp = state.GetState("TownhallHp");
            state.AddState("TownhallHp", townhallHp - 20);
            // neu khoang cach giua townhall va defense nho thì player bị trừ máu
        }

        public override bool CheckPrecondition(WorldState_v2 state)
        {
            return (
                    state.GetState("PlayerState") == (int)PlayerState.MoveToTownhall
                    || state.GetState("PlayerState") == (int)PlayerState.AttackTownhall
                )
                && state.GetState("TownhallHp") > 0
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
            _target = worldState.GetBuffer("Townhall") as GameObject;
            _interval = 1;

            _target.GetComponent<BattleBuilding>().TakeDamage(5);
        }
    }
}
