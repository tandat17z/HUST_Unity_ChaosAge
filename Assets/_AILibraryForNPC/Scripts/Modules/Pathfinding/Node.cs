// Core/Pathfinding/Node.cs
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool IsWalkable;
    public Vector3 WorldPosition;
    public int GridX, GridY;

    public int GCost;
    public int HCost;
    public Node Parent;

    public int HeapIndex { get; set; }

    public int FCost => GCost + HCost;

    public Node(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        IsWalkable = isWalkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(other.HCost);
        }
        return compare;
    }
}
