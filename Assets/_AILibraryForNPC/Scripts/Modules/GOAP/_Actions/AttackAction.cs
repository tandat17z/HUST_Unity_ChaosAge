using System.Collections.Generic;
using ChaosAge.Battle;
using ChaosAge.Data;
using ChaosAge.manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP.Actions
{
    public class AttackAction : GOAPAction
    {
        [SerializeField, ReadOnly]
        private BattleBuilding targetBuilding;

        private float countTime = 0;
        private float shootInterval = 2f;

        protected override void OnAwake() { }

        public override void PrePerform(WorldState worldState)
        {
            var findBuildingSensor = worldState.GetSensor<FindBuildingSensor>();
            if (findBuildingSensor != null)
            {
                targetBuilding = findBuildingSensor.targetBuilding;
                countTime = shootInterval;
            }
        }

        public override void Perform(WorldState worldState)
        {
            if (targetBuilding != null)
            {
                countTime -= Time.deltaTime;
                if (countTime <= 0)
                {
                    // TODO: Tấn công
                    if (GetComponent<BattleUnit>().Type == EUnitType.archer)
                    {
                        var projectile = FactoryManager.Instance.SpawnProjectile(
                            TargetType.building
                        );
                        projectile.Move(
                            transform.position,
                            targetBuilding.transform.position,
                            () =>
                            {
                                targetBuilding.TakeDamage(5);
                            }
                        );
                    }
                    else
                    {
                        targetBuilding.TakeDamage(5);
                    }
                    countTime = shootInterval;
                }
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
