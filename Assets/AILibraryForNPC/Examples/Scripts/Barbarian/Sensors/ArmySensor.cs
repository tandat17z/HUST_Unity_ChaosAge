using AILibraryForNPC.Core;
using ChaosAge.AI.battle;

namespace AILibraryForNPC.Examples
{
    public class ArmySensor : BaseSensor_v2
    {
        private float _countUnits;

        public override void Observe(WorldState_v2 worldstate)
        {
            _countUnits = AIBattleManager.Instance.units.Count;
            worldstate.AddState("countUnits", GetCountUnitLevel(_countUnits));
        }

        private int GetCountUnitLevel(float countUnit)
        {
            if (countUnit <= 3)
                return 0;
            if (countUnit <= 6)
                return 1;
            return 2;
        }
    }
}
