using System.Collections.Generic;

namespace AILibraryForNPC.core.Modules.Pathfinding
{
    public interface IGraph<T>
        where T : INode
    {
        IEnumerable<T> GetNeighbors(T node);
        float GetCost(T from, T to);
        float GetHeuristic(T from, T to);
    }

    public interface INode
    {
        bool IsWalkable { get; }
    }
}
