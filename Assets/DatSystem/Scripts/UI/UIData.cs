using System.Collections.Generic;
using UnityEngine;

namespace DatSystem.UI
{
    public class UIData
    {
        private Dictionary<string, object> _data;

        public UIData()
        {
            _data = new Dictionary<string, object>();
        }

        public UIData(int capacity)
        {
            _data = new Dictionary<string, object>(capacity);
        }

        public void Add(string key, object data)
        {
            _data.Add(key, data);
        }

        public void Add(UIDataKey key, object data)
        {
            _data.Add(key.ToString(), data);
        }

        public T Get<T>(UIDataKey key)
        {
            return Get<T>(key.ToString());
        }

        public bool TryGet<T>(UIDataKey key, out T datum)
        {
            return TryGet(key.ToString(), out datum);
        }

        public T Get<T>(string key)
        {
            object datum = Get(key);

            try
            {
                return (T)datum;
            }
            catch
            {
                throw new System.Exception($"Could not cast data object '{key}' to type '{typeof(T).Name}'");
            }
        }

        public object Get(string key)
        {
            object datum;

            if (!_data.TryGetValue(key, out datum))
                Debug.LogError($"No object found for key '{key}'");

            return datum;
        }

        public bool TryGet(string key, out object datum)
        {
            return _data.TryGetValue(key, out datum);
        }

        public bool TryGet<T>(string key, out T datum)
        {
            object datumObj;

            if (_data.TryGetValue(key, out datumObj))
            {
                try
                {
                    datum = (T)datumObj;
                    return true;
                }
                catch
                {
                    throw new System.Exception($"Could not cast data object '{key}' to type '{typeof(T).Name}'");
                }
            }

            datum = default;
            return false;
        }
    }


    public class UITrackingData
    {
        public string panelName;
        public Dictionary<string, string> trackingParams;
    }


    public enum UIDataKey
    {
        CallBackOnClose,
        TrackingData,
        Content
    }
}