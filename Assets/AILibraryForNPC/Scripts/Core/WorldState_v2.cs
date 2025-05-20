using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.Core
{
    public class WorldState_v2
    {
        private Dictionary<string, float> _state;
        private Dictionary<string, Object> _buffer;

        public WorldState_v2()
        {
            _state = new Dictionary<string, float>();
            _buffer = new Dictionary<string, Object>();
        }

        public void AddState(string key, float value)
        {
            if (_state.ContainsKey(key))
            {
                // Debug.LogWarning($"WorldState_v2: {key} already exists");
                return;
            }
            _state[key] = value;
        }

        public void RemoveState(string key)
        {
            if (!_state.ContainsKey(key))
            {
                return;
            }
            _state.Remove(key);
        }

        public void AddBuffer(string key, Object value)
        {
            if (_buffer.ContainsKey(key))
            {
                // Debug.LogWarning($"WorldState_v2: {key} already exists");
                return;
            }
            _buffer[key] = value;
        }

        public float GetState(string key)
        {
            if (!_state.ContainsKey(key))
            {
                // Debug.LogWarning($"WorldState_v2: {key} does not exist");
                return 0;
            }
            return _state[key];
        }

        public Object GetBuffer(string key)
        {
            if (!_buffer.ContainsKey(key))
            {
                return null;
            }
            return _buffer[key];
        }

        public string GetStateKey()
        {
            return string.Join("_", _state.Values);
        }

        public Dictionary<string, float> GetStates()
        {
            return _state;
        }

        public string GetString()
        {
            string result = "";
            foreach (var item in _state)
            {
                result += $"{item.Key}: {item.Value}   ";
            }
            return result;
        }

        public WorldState_v2 Clone()
        {
            var clone = new WorldState_v2();
            foreach (var item in _state)
            {
                clone.AddState(item.Key, item.Value);
            }
            foreach (var item in _buffer)
            {
                clone.AddBuffer(item.Key, item.Value);
            }
            return clone;
        }
    }
}
