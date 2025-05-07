using System;
using System.Collections.Generic;
using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public abstract class GOAPAction : BaseAction_v2
    {
        public abstract Dictionary<string, float> GetPrecondition();
        public abstract Dictionary<string, float> GetEffect();

        public bool IsAchievableGiven(Dictionary<string, float> state)
        {
            foreach (var precondition in GetPrecondition())
            {
                if (!state.ContainsKey(precondition.Key))
                {
                    return false;
                }
                if (state[precondition.Key] < precondition.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public abstract float GetCost();
    }
}
