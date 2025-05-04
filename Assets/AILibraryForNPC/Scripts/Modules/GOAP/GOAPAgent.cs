using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    [RequireComponent(typeof(GOAPActionSystem))]
    public abstract class GOAPAgent : BaseAgent
    {
        [SerializeField]
        private GOAPGoalSystem _goalSystem;

        public GOAPGoalSystem GoalSystem => _goalSystem;
    }
}
