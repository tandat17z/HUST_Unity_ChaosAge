using System;
using System.Collections.Generic;
using AILibraryForNPC.core.Base;

namespace AILibraryForNPC.core
{
    public class WorldState
    {
        public Dictionary<string, BaseSensor> dataSensors = new Dictionary<string, BaseSensor>();
        public Dictionary<string, int> states = new Dictionary<string, int>();

        public WorldState() { }

        public void SetState(string key, int value)
        {
            states[key] = value;
        }

        public object GetState(string key)
        {
            return states[key];
        }

        public bool ContainsKey(string key)
        {
            return states.ContainsKey(key);
        }

        public Dictionary<string, int> GetStates()
        {
            return states;
        }

        public void ModifyState(string key, int value)
        {
            if (states.ContainsKey(key))
            {
                states[key] += value;
            }
            else
            {
                states.Add(key, value);
            }
        }

        public T GetSensor<T>()
            where T : BaseSensor
        {
            return dataSensors[typeof(T).Name] as T;
        }

        public void AddSensor(BaseSensor sensor)
        {
            dataSensors[sensor.GetType().Name] = sensor;
        }
    }
}
