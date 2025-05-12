using System.Collections.Generic;

namespace AILibraryForNPC.Algorithms
{
    public class DefaultGOAPHeuristic : IGOAPHeuristic
    {
        public float CalculateHeuristic(
            Dictionary<string, float> state,
            Dictionary<string, float> goal
        )
        {
            float h = 0;
            foreach (var goalItem in goal)
            {
                if (!state.ContainsKey(goalItem.Key))
                {
                    h += goalItem.Value;
                }
                else if (state[goalItem.Key] < goalItem.Value)
                {
                    h += goalItem.Value - state[goalItem.Key];
                }
            }
            return h;
        }
    }
}
