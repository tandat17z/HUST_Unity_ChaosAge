using System.Collections.Generic;
using AILibraryForNPC.Core;

namespace AILibraryForNPC.Modules.GOAP
{
    public abstract class GOAPAction : BaseAction_v2
    {
        public abstract Dictionary<string, float> GetPrecondition();
        public abstract Dictionary<string, float> GetEffect();
    }
}
