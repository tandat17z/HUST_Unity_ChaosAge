using UnityEngine;

namespace AILibraryForNPC.core.Modules.GOAP
{
    [RequireComponent(typeof(GOAPGoalSystem))]
    [RequireComponent(typeof(GOAPActionSystem))]
    public class GOAPAgent : Agent
    {
        public override void Initialize() { }
    }
}
