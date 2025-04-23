using System;
using System.Collections.Generic;

namespace GOAPSystem
{
    [Serializable]
    public class WorldState
    {
        public string key;
        public int value;
    }

    public class WorldStates
    {
        public Dictionary<string, int> states;

        public WorldStates()
        {
            states = new Dictionary<string, int>();
        }

        void AddState(string key, int value)
        {
            states.Add(key, value);
        }

        public bool HasState(string key)
        {
            return states.ContainsKey(key);
        }

        public void ModifyState(string key, int value)
        {
            if (states.ContainsKey(key))
            {
                states[key] += value;
                if (states[key] <= 0)
                {
                    RemoveState(key);
                }
            }
            else
            {
                states.Add(key, value);
            }
        }

        void RemoveState(string key)
        {
            states.Remove(key);
        }

        public void SetState(string key, int value)
        {
            if (states.ContainsKey(key))
            {
                states[key] = value;
            }
            else
            {
                states.Add(key, value);
            }
        }

        public Dictionary<string, int> GetStates()
        {
            return states;
        }
    }
}
