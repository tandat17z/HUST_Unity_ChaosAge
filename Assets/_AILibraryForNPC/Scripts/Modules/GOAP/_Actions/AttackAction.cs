using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP.Actions
{
    public class AttackAction : GOAPAction
    {
        [SerializeField, ReadOnly]
        private BattleBuilding targetBuilding;

        protected override void OnAwake() { }

        public override void PrePerform(WorldState worldState)
        {
            var findBuildingSensor = worldState.GetSensor<FindBuildingSensor>();
            if (findBuildingSensor != null)
            {
                targetBuilding = findBuildingSensor.targetBuilding;
            }
        }

        public override void Perform(WorldState worldState)
        {
            if (targetBuilding != null)
            {
                targetBuilding.TakeDamage(10 * Time.deltaTime);
            }
        }

        public override void PostPerform(WorldState worldState)
        {
            worldState.states = new Dictionary<string, int>();
        }

        public override bool IsActionComplete(WorldState worldState)
        {
            return targetBuilding == null;
        }
    }
}
