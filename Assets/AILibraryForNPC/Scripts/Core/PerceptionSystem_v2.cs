using System;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.Core
{
    public class PerceptionSystem_v2
    {
        private List<BaseSensor_v2> _sensors = new();

        public void AddSensor(BaseSensor_v2 sensor)
        {
            _sensors.Add(sensor);
        }

        public void InitializeSensors(BaseAgent agent)
        {
            foreach (var sensor in _sensors)
            {
                sensor.Initialize(agent);
            }

            Debug.LogWarning("Complete Register: " + _sensors.Count);
        }

        public WorldState_v2 Observe()
        {
            WorldState_v2 worldState = new WorldState_v2();
            foreach (var sensor in _sensors)
            {
                sensor.Observe(worldState);
            }
            return worldState;
        }
    }
}
