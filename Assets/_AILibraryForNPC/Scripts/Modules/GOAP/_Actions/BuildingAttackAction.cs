using System.Collections.Generic;
using ChaosAge.Battle;
using ChaosAge.manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP.Actions
{
    public class BuildingAttackAction : GOAPAction
    {
        [SerializeField, ReadOnly]
        private BattleUnit targetSoldier;
        private float shootInterval = 1f;
        private float countTime = 0;

        protected override void OnAwake() { }

        public override void PrePerform(WorldState worldState)
        {
            var findSoldierSensor = worldState.GetSensor<FindSoldierSensor>();
            if (findSoldierSensor != null)
            {
                targetSoldier = findSoldierSensor.targetSoldier;
                countTime = shootInterval;
            }
        }

        public override void Perform(WorldState worldState)
        {
            if (targetSoldier != null)
            {
                countTime -= Time.deltaTime;
                if (countTime <= 0)
                {
                    // TODO: Tấn công
                    var projectile = FactoryManager.Instance.SpawnProjectile(TargetType.unit);
                    projectile.Move(
                        transform.position,
                        targetSoldier.transform.position,
                        () =>
                        {
                            Debug.Log("AttackAction" + targetSoldier.name);
                            targetSoldier.TakeDamage(10);
                        }
                    );

                    Debug.LogWarning("spawn projectile");
                    countTime = shootInterval;
                }
            }
        }

        public override void PostPerform(WorldState worldState) { }

        public override bool IsActionComplete(WorldState worldState)
        {
            return targetSoldier == null;
        }
    }
}
