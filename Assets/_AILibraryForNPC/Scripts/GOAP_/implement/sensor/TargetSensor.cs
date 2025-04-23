using AILibraryForNPC.core;
using ChaosAge.AI.battle;
using ChaosAge.manager;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetSensor", menuName = "GOAP/Sensor/TargetSensor")]
public class TargetSensor : BaseSensorSO
{
    public override void UpdateSensor(Agent agent, AILibraryForNPC.core.WorldState state)
    {
        //     var instanceState = state as MoveWorldState;
        //     var posCell = BuildingManager.Instance.Grid.ConvertGridPos(agent.transform.position);
        //     instanceState.X = (int)posCell.x;
        //     instanceState.Y = (int)posCell.y;

        //     var minDis = float.MaxValue;
        //     foreach (var building in AIBattleManager.Instance.buildings)
        //     {
        //         var dis = Vector3.Distance(agent.transform.position, building.transform.position);
        //         if (dis < minDis)
        //         {
        //             minDis = dis;

        //             posCell = BuildingManager.Instance.Grid.ConvertGridPos(building.transform.position);
        //             instanceState.TargetX = (int)posCell.x;
        //             instanceState.TargetY = (int)posCell.y;
        //             instanceState.TargetPosition = building.transform.position;
        //         }
        //     }
    }
}
