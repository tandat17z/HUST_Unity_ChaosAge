using AILibraryForNPC.Core;
using ChaosAge.manager;
using UnityEngine;

public class LocationSensor : BaseSensor_v2
{
    public override void Observe(WorldState_v2 worldstate)
    {
        var gridPosition = GetGridPosition(agent.transform.position);
        worldstate.AddState("locationX", gridPosition.x);
        worldstate.AddState("locationY", gridPosition.z);
    }

    public Vector3 GetGridPosition(Vector3 pos){
        return BuildingManager.Instance.Grid.ConvertGridPos(pos);
    }
}
