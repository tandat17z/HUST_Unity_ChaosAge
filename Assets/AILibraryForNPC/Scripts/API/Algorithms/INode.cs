using System;
using System.Collections.Generic;

namespace AILibraryForNPC.Algorithms
{
    public interface INode
    {
        public List<INode> GetNeighbors();

        public bool Equals(INode other);

        public float GetCost();

        public float GetHeuristic(INode node);

        public bool CheckIfGoalReached(INode current);
    }
}
