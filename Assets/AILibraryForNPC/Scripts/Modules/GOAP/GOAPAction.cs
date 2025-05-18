using System;
using System.Collections.Generic;
using AILibraryForNPC.Core;
using UnityEngine;

namespace AILibraryForNPC.Modules.GOAP
{
    public abstract class GOAPAction : BaseAction_v2
    {
        // public virtual bool IsAchievableGiven(WorldState_v2 state)
        // {
        //     foreach (var precondition in GetPrecondition())
        //     {
        //         if (!state.GetStates().ContainsKey(precondition.Key))
        //         {
        //             return false;
        //         }
        //         if (state.GetStates()[precondition.Key] < precondition.Value)
        //         {
        //             return false;
        //         }
        //     }
        //     return true;
        // }

        public abstract float GetCost();

        public abstract bool CheckPrecondition(WorldState_v2 state);

        public abstract void ApplyEffect(WorldState_v2 state);

        public virtual void Cancel() { }
    }
}
