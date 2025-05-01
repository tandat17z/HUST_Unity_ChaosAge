using System.Collections.Generic;
using AILibraryForNPC.core.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public class PerceptionSystem : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        protected List<BaseSensor> sensors = new List<BaseSensor>();
        protected WorldState worldState;

        void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            worldState = new WorldState();
        }

        protected void Start()
        {
            sensors.AddRange(GetComponents<BaseSensor>());
            foreach (var sensor in sensors)
            {
                sensor.Initialize(worldState);
                worldState.AddSensor(sensor);
            }
        }

        public WorldState UpdateWorldState()
        {
            foreach (var sensor in sensors)
            {
                sensor.UpdateSensor();
            }
            return worldState;
        }
    }
}
