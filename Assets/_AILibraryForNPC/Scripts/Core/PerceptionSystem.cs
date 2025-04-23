using System.Collections.Generic;
using AILibraryForNPC.core.Base;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public class PerceptionSystem : MonoBehaviour
    {
        private List<BaseSensor> sensors = new List<BaseSensor>();
        private WorldState worldState;

        protected virtual void Awake()
        {
            // Lấy tất cả các sensor được gắn vào GameObject
            sensors.AddRange(GetComponents<BaseSensor>());
        }

        public void Initialize()
        {
            worldState = new WorldState();
            foreach (var sensor in sensors)
            {
                sensor.Initialize(worldState);
            }
        }

        public WorldState GetWorldState()
        {
            foreach (var sensor in sensors)
            {
                sensor.UpdateSensor();
            }
            return worldState;
        }
    }
}
