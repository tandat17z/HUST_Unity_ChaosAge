using AILibraryForNPC.Core;
using ChaosAge.Battle;

namespace AILibraryForNPC.Examples
{
    public class BarbarianSensor : BaseSensor_v2
    {
        private float _health;

        public override void Observe(WorldState_v2 worldstate)
        {
            _health = agent.GetComponent<BattleUnit>().Health;
            worldstate.AddState("health", GetHealthLevel(_health));
        }

        public int GetHealthLevel(float health)
        {
            if (health <= 20)
                return 0;
            return 1;
        }
    }
}
