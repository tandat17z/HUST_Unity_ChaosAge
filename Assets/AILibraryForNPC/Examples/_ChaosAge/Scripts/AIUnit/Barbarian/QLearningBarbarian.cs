using AILibraryForNPC.Modules.QLearning;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class QLearningBarbarian : QLearningAgent
    {
        public override void OnAwake()
        {
            GetComponent<NavMeshAgent>().updateRotation = false;
        }

        public override void RegisterActions()
        {
            actionSystem.AddAction(new QLearningMoveToDefense());
            actionSystem.AddAction(new QLearningMoveToTownhall());
            actionSystem.AddAction(new QLearningAttack());
        }

        public override void RegisterSensors()
        {
            perceptionSystem.AddSensor(new BarbarianSensor());
            // perceptionSystem.AddSensor(new ArmySensor());
            perceptionSystem.AddSensor(new BuildingSensor());
        }
    }
}
