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

        public override void RegisterGoals()
        {
            goalSystem.AddGoal(new GoalDefense());
            goalSystem.AddGoal(new GoalGoHome());
            goalSystem.AddGoal(new GoalTownhall());

            Debug.Log("RegisterGoals" + goalSystem.goals.Count);
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
            perceptionSystem.AddSensor(new GoapBuildingSensor());
            perceptionSystem.AddSensor(new GoapSensor());
        }
    }

    public enum PlayerState
    {
        Idle,
        Home,
        MoveToDefense,
        MoveToTownhall,
        Attack,
    }
}
