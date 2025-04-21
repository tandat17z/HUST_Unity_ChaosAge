using AILibraryForNPC.core;

public abstract class GoapWorldState : WorldState
{
    public abstract bool IsSatisfiedBy(GoapWorldState goalState);

    public abstract int CustomEquals(GoapWorldState other);

    public abstract int GetHashCode();
}
