using System.Collections.Generic;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public abstract class GOAPAction : BaseAction
    {
        public float cost = 1.0f;
        public List<TargetState> listPreconditions;
        public List<TargetState> listEffects;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        protected virtual void Awake()
        {
            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
            foreach (var precondition in listPreconditions)
            {
                preconditions.Add(precondition.key, precondition.value);
            }
            foreach (var effect in listEffects)
            {
                effects.Add(effect.key, effect.value);
            }
            OnAwake();
        }

        protected abstract void OnAwake();

        public Dictionary<string, int> GetEffects()
        {
            return effects;
        }

        public bool IsAchievableGiven(Dictionary<string, int> conditions)
        {
            foreach (var precondition in preconditions)
            {
                if (!conditions.ContainsKey(precondition.Key))
                    return false;
            }
            return true;
        }
    }
}
