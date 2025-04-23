using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP.Actions
{
    public class AttackAction : GOAPAction
    {
        [SerializeField]
        private bool isBuildingDied = false;

        public override void PrePerform(WorldState worldState)
        {
            isBuildingDied = false;
        }

        public override void Perform(WorldState worldState)
        {
            Debug.Log("Attack");
        }

        public override void PostPerform(WorldState worldState)
        {
            worldState.states = new Dictionary<string, int>();
        }

        public override bool IsActionComplete(WorldState worldState)
        {
            return isBuildingDied == true;
        }
    }
}
