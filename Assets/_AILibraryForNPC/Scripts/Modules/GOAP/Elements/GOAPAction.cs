using System.Collections.Generic;

namespace AILibraryForNPC.core.Modules.GOAP
{
    public abstract class GOAPAction : BaseAction
    {
        public float cost = 1.0f;
        public Dictionary<string, int> preconditions = new Dictionary<string, int>();
        public Dictionary<string, int> effects = new Dictionary<string, int>();

        public override string ActionName => GetType().Name;
        protected bool isExecuting = false;

        protected virtual void Awake()
        {
            InitializePreconditions();
            InitializeEffects();
        }

        protected abstract void InitializePreconditions();
        protected abstract void InitializeEffects();

        public bool IsActionComplete()
        {
            return !isExecuting;
        }

        public virtual bool PrePerform()
        {
            isExecuting = true;
            return true;
        }

        public virtual void PostPerform()
        {
            isExecuting = false;
        }

        public abstract void Perform();

        public Dictionary<string, int> GetPreconditions()
        {
            return preconditions;
        }

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
