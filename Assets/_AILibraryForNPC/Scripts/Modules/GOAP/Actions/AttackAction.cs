using AILibraryForNPC.core.Modules.GOAP;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP.Actions
{
    public class AttackAction : GOAPAction
    {
        [SerializeField]
        private bool isBuildingDied = false;

        protected override void InitializePreconditions()
        {
            preconditions.Add("hasTarget", 1);
            preconditions.Add("isAtTarget", 1);
        }

        protected override void InitializeEffects()
        {
            effects.Add("targetDestroyed", 1);
        }

        public override void Perform()
        {
            Debug.Log("Attack");
            // Thêm logic tấn công ở đây
            // Khi building bị phá hủy, set isBuildingDied = true
        }

        public override bool PrePerform()
        {
            isBuildingDied = false;
            return true;
        }

        public override void PostPerform()
        {
            isBuildingDied = false;
        }

        public bool IsActionComplete()
        {
            return isBuildingDied;
        }

        public override void PrePerform(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }

        public override void Perform(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }

        public override void PostPerform(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsActionComplete(WorldState worldState)
        {
            throw new System.NotImplementedException();
        }
    }
}
