using AILibraryForNPC.core;
using ChaosAge.manager;
using UnityEngine;

public class AIAgent : Agent
{
    public void Initialize(int x, int y)
    {
        var position = BattleVector2.GridToWorldPosition(new BattleVector2Int(x, y));
        var pos = BuildingManager.Instance.Grid.transform.TransformPoint(
            new Vector3(position.x, 0, position.y)
        );
        transform.position = pos;
    }
}
