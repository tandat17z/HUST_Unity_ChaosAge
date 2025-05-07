using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
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
            actionSystem.AddAction(new GOAPMoveToDefense());
            actionSystem.AddAction(new GOAPMoveToTownhall());
            actionSystem.AddAction(new GOAPAttack());
        }

        public override void RegisterSensors()
        {
            perceptionSystem.AddSensor(new BarbarianSensor());
            perceptionSystem.AddSensor(new ArmySensor());
            perceptionSystem.AddSensor(new BuildingSensor());
        }
    }
}
