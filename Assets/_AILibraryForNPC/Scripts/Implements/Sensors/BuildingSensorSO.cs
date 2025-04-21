using AILibraryForNPC.core;
using AILibraryForNPC.core.Modules.Pathfinding;
using ChaosAge.AI.battle;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSensor", menuName = "AI/NPC/Sensors/BuildingSensor")]
public class BuildingSensorSO : BaseSensorSO
{
    public override void UpdateSensor(Agent agent, WorldState state)
    {
        var buildings = AIBattleManager.Instance.buildings;

        var gridGraph = new GridGraph(40, 40, 1);
        foreach (var building in buildings)
        {
            for (int i = 0; i < building.battleBuidlingConfig.columns; i++)
            {
                for (int j = 0; j < building.battleBuidlingConfig.columns; j++)
                {
                    gridGraph.SetNodeWalkable(
                        building.GridPosition.x + i,
                        building.GridPosition.y + j,
                        false
                    );
                }
            }
        }
        state.SetState("gridGraph", gridGraph);
    }
}
