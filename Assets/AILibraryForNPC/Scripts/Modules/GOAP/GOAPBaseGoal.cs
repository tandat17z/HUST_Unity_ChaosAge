using System;
using AILibraryForNPC.Algorithms;
using AILibraryForNPC.Core;

namespace AILibraryForNPC.GOAP
{
    public abstract class GOAPBaseGoal
    {
        public abstract string GetName();
        public abstract bool IsGoalReached(WorldState_v2 worldState);
        public abstract float GetWeight(WorldState_v2 worldState);
        public abstract float GetHeuristic(WorldState_v2 worldState);
    }
}
