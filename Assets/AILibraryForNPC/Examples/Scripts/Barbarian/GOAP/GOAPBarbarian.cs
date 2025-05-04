using AILibraryForNPC.Modules.GOAP;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPBarbarian : GOAPAgent
    {
        public override void OnAwake()
        {
            GetComponent<NavMeshAgent>().updateRotation = false;
        }

        public override void RegisterActions()
        {
            // actionSystem.AddAction(new MoveToDefense());
            // actionSystem.AddAction(new MoveToTownhall());
            // actionSystem.AddAction(new Attack());
        }

        public override void RegisterSensors()
        {
            // perceptionSystem.AddSensor(new BarbarianSensor());
            // perceptionSystem.AddSensor(new ArmySensor());
            // perceptionSystem.AddSensor(new BuildingSensor());
        }
    }
}
