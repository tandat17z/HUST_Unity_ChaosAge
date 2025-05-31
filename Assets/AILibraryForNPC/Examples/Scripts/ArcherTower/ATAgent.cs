using AILibraryForNPC.Core;
using UnityEngine;

[RequireComponent(typeof(ATActionSystem))]
public class ATAgent : BaseAgent
{
    public override void OnAwake() { }

    public override void RegisterActions()
    {
        actionSystem.AddAction(new ATAttack());
    }

    public override void RegisterSensors()
    {
        perceptionSystem.AddSensor(new ATSensor());
    }
}
