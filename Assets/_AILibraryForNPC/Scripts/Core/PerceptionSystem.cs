namespace AILibraryForNPC.core
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PerceptionSystem : MonoBehaviour
    {
        [SerializeField] private List<BaseSensorSO> sensors;

        private WorldState _worldState;

        public void Initialize()
        {

        }

        public WorldState GetWorldState()
        {
            foreach (var sensor in sensors)
            {
                sensor.UpdateSensor(GetComponent<Agent>(), _worldState);
            }
            return _worldState;
        }
    }


    public interface ISensor
    {
        void UpdateSensor(WorldState state);
    }

    public class EnemySensor : ISensor
    {
        private MonoBehaviour context;

        public EnemySensor(MonoBehaviour context)
        {
            this.context = context;
        }

        public void UpdateSensor(WorldState state)
        {
            state.EnemyInSight = UnityEngine.Random.value > 0.5f;
        }
    }

    public class HealthSensor : ISensor
    {
        private MonoBehaviour context;

        public HealthSensor(MonoBehaviour context)
        {
            this.context = context;
        }

        public void UpdateSensor(WorldState state)
        {
            //var healthComp = context.GetComponent<Health>();
            //state.Health = healthComp != null ? healthComp.CurrentHealth : 100;
        }
    }

    public class WorldState
    {
        public bool EnemyInSight;
        public float Health;
        // Add more environmental factors as needed
    }
}
