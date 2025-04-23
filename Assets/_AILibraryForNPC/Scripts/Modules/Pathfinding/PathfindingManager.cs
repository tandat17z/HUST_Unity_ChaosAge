using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.Pathfinding
{
    public class PathfindingManager
    {
        public static PathfindingManager Instance;

        public GridMap GridMap;

        private void Awake()
        {
            Instance = this;
        }

        public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            // return AStarPathfinder<GridNode>.Instance.FindPath(startPos, targetPos, GridMap);
            return null;
        }
    }
}
