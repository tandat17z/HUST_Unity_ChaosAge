using System;
using System.Collections.Generic;

namespace AILibraryForNPC.core
{
    public class WorldState
    {
        public Dictionary<string, int> states = new Dictionary<string, int>();

        public WorldState() { }

        public WorldState(WorldState state)
        {
            states = new Dictionary<string, int>(state.states);
            foreach (var st in state.states)
            {
                states.Add(st.Key, st.Value);
            }
        }

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
    }
}
