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
            worldstate.AddState("countUnits", _countUnits);
        }
    }
}
