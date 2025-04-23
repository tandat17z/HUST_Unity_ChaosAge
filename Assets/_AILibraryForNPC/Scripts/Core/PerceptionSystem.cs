using System.Collections.Generic;
using AILibraryForNPC.core.Base;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public class PerceptionSystem : MonoBehaviour
    {
        [SerializeField]
        private List<BaseSensor> sensors = new List<BaseSensor>();
        private WorldState worldState;

        protected virtual void Start()
        {
            worldState = new WorldState();
            foreach (var sensor in sensors)
            {
                sensor.Initialize(worldState);
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
