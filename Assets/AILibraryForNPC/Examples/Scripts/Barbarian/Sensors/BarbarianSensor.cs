using AILibraryForNPC.Core;
using ChaosAge.Battle;

namespace AILibraryForNPC.Examples
{
    public class BarbarianSensor : BaseSensor_v2
    {
        private float _health;

        public override void Observe(WorldState_v2 worldstate)
        {
            _health = agent.GetComponent<BattleUnit>().health;
            worldstate.AddState("health", GetHealthLevel(_health));
        }

        public int GetHealthLevel(float health)
        {
            if (health <= 40)
                return 0;
            return 1;
        }
    }
}
