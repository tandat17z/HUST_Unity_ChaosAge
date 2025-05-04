using System;
using System.Collections.Generic;

namespace AILibraryForNPC.Core
{
    public class PerceptionSystem_v2
    {
        private List<BaseSensor_v2> _sensors;

        public PerceptionSystem_v2()
        {
            _sensors = new List<BaseSensor_v2>();
        }

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
