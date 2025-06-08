using AILibraryForNPC.Core;
using AILibraryForNPC.Examples;
using ChaosAge.AI.battle;
using ChaosAge.Battle;
using UnityEngine;

public class GoapSensor : BaseSensor_v2
{
    public override void Observe(WorldState_v2 worldstate)
    {
        worldstate.AddState("PlayerState", (int)PlayerState.Idle);

        var hp = agent.GetComponent<BattleUnit>().Health;
        worldstate.AddState("PlayerHp", hp);

        var home = AIBattleManager.Instance.home;
        worldstate.AddBuffer("Home", home);

        if (Vector3.Distance(agent.transform.position, home.transform.position) < 1)
        {
            worldstate.AddState("PlayerState", (int)PlayerState.Home);
        }
    }
}
