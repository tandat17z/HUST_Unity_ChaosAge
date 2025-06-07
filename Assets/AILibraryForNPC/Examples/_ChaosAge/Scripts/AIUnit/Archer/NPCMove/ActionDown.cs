using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using ChaosAge.AI.battle;
using UnityEngine;

public class ActionDown : GOAPAction
{
    private float stepTime;

    public override void ApplyEffect(WorldState_v2 state)
    {
        // cập nhật tới ô tiếp theo
        // đánh đấu những ô đã đi qua vừa thực hiện
        // state["locationY"] -= 1;

        // var pos = $"{state["locationX"]}_{state["locationY"]}";
        // state[pos] = 1;
    }

    public override bool CheckPrecondition(WorldState_v2 state)
    {
        // hướng đi trước đó không phải là up
        // ô tiếp theo có thể di chuyển
        // var nextX = state["locationX"];
        // var nextY = state["locationY"] - 1;
        // var pos = $"{nextX}_{nextY}";
        // if (state[pos] != 0 || AIBattleManager.Instance.CanMove(nextX, nextY) == false)
        // {
        //     return false;
        // }
        return true;
    }

    public override float GetCost()
    {
        return 1;
    }
    
    public override bool IsComplete(WorldState_v2 worldState)
    {
        return stepTime <= 0;
    }

    public override void Perform(WorldState_v2 worldState)
    {
        stepTime -= Time.deltaTime;
    }

    public override void PostPerform(WorldState_v2 worldState)
    {
        agent.transform.position += new Vector3(0, 0, -1);
    }

    public override void PrePerform(WorldState_v2 worldState)
    {
        stepTime = 1;
    }
}
