// Core/Pathfinding/IHeapItem.cs
public interface IHeapItem<T> : System.IComparable<T>
{
    int HeapIndex { get; set; }
}
