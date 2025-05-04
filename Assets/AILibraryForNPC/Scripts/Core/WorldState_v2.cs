using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.Core
{
    public class WorldState_v2
    {
        private Dictionary<string, float> _state;

        public WorldState_v2()
        {
            _state = new Dictionary<string, float>();
        }

        public void AddState(string key, float value)
        {
            if (_state.ContainsKey(key))
            {
                Debug.LogError($"WorldState_v2: {key} already exists");
                return;
            }
            _state[key] = value;
        }

        public float GetState(string key)
        {
            if (!_state.ContainsKey(key))
            {
                Debug.LogError($"WorldState_v2: {key} does not exist");
                return 0;
            }
            return _state[key];
        }

        public string GetStateKey()
        {
            return string.Join("_", _state.Keys);
        }
    }
}
