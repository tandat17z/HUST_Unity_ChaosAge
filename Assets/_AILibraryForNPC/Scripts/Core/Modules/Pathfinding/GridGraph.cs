using System;
using System.Collections.Generic;
using UnityEngine;

namespace AILibraryForNPC.core.Modules.Pathfinding
{
    public class GridNode : INode
    {
        public Vector2Int GridPosition { get; set; }
        public Vector3 WorldPosition { get; set; }
        public bool IsWalkable { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is GridNode other)
                return GridPosition == other.GridPosition;
            return false;
        }

        public override int GetHashCode()
        {
            return GridPosition.GetHashCode();
        }
    }

    public class GridGraph : IGraph<GridNode>
    {
        private GridNode[,] grid;
        private float nodeSize;

        public GridGraph(int width, int height, float nodeSize)
        {
            this.grid = new GridNode[width, height];
            this.nodeSize = nodeSize;
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Vector3 worldPos = new Vector3(x * nodeSize, 0, y * nodeSize);
                    // bool isWalkable = !Physics.CheckSphere(
                    //     worldPos,
                    //     nodeSize * 0.5f,
                    //     obstacleLayer
                    // );

                    bool isWalkable = true;
                    grid[x, y] = new GridNode
                    {
                        GridPosition = new Vector2Int(x, y),
                        WorldPosition = worldPos,
                        IsWalkable = isWalkable,
                    };
                }
            }
        }

        public IEnumerable<GridNode> GetNeighbors(GridNode node)
        {
            var neighbors = new List<GridNode>();
            var pos = node.GridPosition;

            // 4 hướng di chuyển
            TryAddNeighbor(pos.x + 1, pos.y, neighbors);
            TryAddNeighbor(pos.x - 1, pos.y, neighbors);
            TryAddNeighbor(pos.x, pos.y + 1, neighbors);
            TryAddNeighbor(pos.x, pos.y - 1, neighbors);

            return neighbors;
        }

        private void TryAddNeighbor(int x, int y, List<GridNode> neighbors)
        {
            if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
            {
                neighbors.Add(grid[x, y]);
            }
        }

        public float GetCost(GridNode from, GridNode to)
        {
            return Vector2Int.Distance(from.GridPosition, to.GridPosition);
        }

        public float GetHeuristic(GridNode from, GridNode to)
        {
            return Vector2Int.Distance(from.GridPosition, to.GridPosition);
        }

        public GridNode GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / nodeSize);
            int y = Mathf.RoundToInt(worldPosition.z / nodeSize);

            if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                return grid[x, y];

            return null;
        }

        public void SetNodeWalkable(int x, int y, bool v)
        {
            grid[x, y].IsWalkable = v;
        }
    }
}
