namespace AILibraryForNPC.core
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PerceptionSystem : MonoBehaviour
    {
        [SerializeField]
        private List<BaseSensorSO> sensors;

        private WorldState _worldState;

        public void Initialize()
        {
            _worldState = new MoveWorldState();
        }

        public WorldState GetWorldState()
        {
            foreach (var sensor in sensors)
            {
                sensor.UpdateSensor(GetComponent<Agent>(), _worldState);
            }
            return _worldState;
        }
    }
}
