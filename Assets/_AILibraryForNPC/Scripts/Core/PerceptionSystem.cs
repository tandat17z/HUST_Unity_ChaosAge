using System.Collections.Generic;
using AILibraryForNPC.core.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public class PerceptionSystem : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private List<BaseSensor> sensors = new List<BaseSensor>();
        private WorldState worldState;

        protected virtual void Start()
        {
            worldState = new WorldState();
            sensors.AddRange(GetComponents<BaseSensor>());
            foreach (var sensor in sensors)
            {
                sensor.Initialize(worldState);
                worldState.AddSensor(sensor);
            }
        }

        public WorldState UpdateWorldState()
        {
            foreach (var sensor in sensors)
            {
                sensor.UpdateSensor();
            }
            return worldState;
        }
    }
}
