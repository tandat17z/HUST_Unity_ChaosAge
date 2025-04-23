using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public class WorldState
    {
        public Dictionary<string, object> states = new Dictionary<string, object>();

        public void SetState(string key, object value)
        {
            states[key] = value;
        }

        public object GetState(string key)
        {
            return states[key];
        }

        public T GetState<T>(string key)
        {
            return (T)states[key];
        }

        public bool ContainsKey(string key)
        {
            return states.ContainsKey(key);
        }

        internal Dictionary<string, int> GetStates()
        {
            throw new NotImplementedException();
        }
    }
}
