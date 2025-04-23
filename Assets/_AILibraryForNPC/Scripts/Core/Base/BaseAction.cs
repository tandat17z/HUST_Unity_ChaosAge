using System;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core
{
    public abstract class BaseAction : MonoBehaviour
    {
        public abstract string ActionName { get; }

        public abstract void PrePerform(WorldState worldState);
        public abstract void Perform(WorldState worldState);
        public abstract void PostPerform(WorldState worldState);
        public abstract bool IsActionComplete(WorldState worldState);
    }
}
