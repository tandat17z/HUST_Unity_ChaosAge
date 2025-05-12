using System.Collections.Generic;

namespace AILibraryForNPC.Algorithms
{
    public interface IGOAPHeuristic
    {
        float CalculateHeuristic(Dictionary<string, float> state, Dictionary<string, float> goal);
    }
}
