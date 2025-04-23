using System;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public abstract class BaseActionSO : ScriptableObject
    {
        protected bool isExecuting = false;

        public bool IsExecuting => isExecuting;

        public virtual void StartExecute(Agent agent, WorldState worldState)
        {
            isExecuting = true;
        }

        public abstract void ExecutePerFrame(Agent agent, WorldState worldState);

        public virtual void StopExecute(Agent agent)
        {
            isExecuting = false;
        }
    }
}
