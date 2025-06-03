using System.Collections.Generic;
using DatSystem.utils;
using UnityEngine;

public class SOManager : Singleton<SOManager>
{
    protected override void OnAwake() { }

    private Dictionary<string, ScriptableObject> _soMap =
        new Dictionary<string, ScriptableObject>();

    public T GetSO<T>(string name = "")
        where T : ScriptableObject
    {
        if (name == "")
        {
            name = typeof(T).Name;
        }
        if (!_soMap.ContainsKey(name))
        {
            _soMap[name] = Resources.Load<T>($"SO/{name}");
        }
        return _soMap[name] as T;
    }
}
