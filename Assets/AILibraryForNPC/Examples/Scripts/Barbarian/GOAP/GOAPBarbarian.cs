using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace AILibraryForNPC.Examples
{
    public class GOAPBarbarian : GOAPAgent
    {
        public PlayerState currentState;
        public override void OnAwake()
        {
            base.OnAwake();
            GetComponent<NavMeshAgent>().updateRotation = false;
        }
        public override void RegisterGoals()
        {
            goalSystem.AddGoal(new GoalDefense());
            goalSystem.AddGoal(new GoalGoHome());
            goalSystem.AddGoal(new GoalTownhall());
        }
        public override void RegisterActions()
        {
            actionSystem.AddAction(new GOAPMoveToDefense());
            actionSystem.AddAction(new GOAPMoveToTownhall());
            actionSystem.AddAction(new GOAPAttackDefense());
            actionSystem.AddAction(new GOAPAttackTownhall());
            actionSystem.AddAction(new GOAPGoHome());
        }
        public override void RegisterSensors()
        {
            perceptionSystem.AddSensor(new GoapSensor());
            perceptionSystem.AddSensor(new GoapBuildingSensor());
        }
        public override bool ConditionCancelPlan(WorldState_v2 worldState)
        {
            // return false;
            return worldState.GetState("PlayerHp") <= 50 && currentState != PlayerState.Home;
        }
    }

    public enum PlayerState
    {
        Idle,
        Home,
        MoveToDefense,
        MoveToTownhall,
        AttackDefense,
        AttackTownhall,
    }
}
