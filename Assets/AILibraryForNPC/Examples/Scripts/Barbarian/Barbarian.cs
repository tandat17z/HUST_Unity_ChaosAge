using AILibraryForNPC.Modules.QLearning;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class Barbarian : QLearningAgent
    {
        public override void OnAwake()
        {
            GetComponent<NavMeshAgent>().updateRotation = false;
        }

        public override void RegisterActions()
        {
            actionSystem.AddAction(new MoveToDefense());
            actionSystem.AddAction(new MoveToTownhall());
            actionSystem.AddAction(new Attack());
        }

        public override void RegisterSensors()
        {
            perceptionSystem.AddSensor(new BarbarianSensor());
            perceptionSystem.AddSensor(new ArmySensor());
            perceptionSystem.AddSensor(new BuildingSensor());
        }
    }
}
