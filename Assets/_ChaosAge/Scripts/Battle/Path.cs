using AStarPathfinding;
using System.Collections.Generic;
using System.Linq;

public class Path
{
    public Path()
    {
        length = 0;
        points = null;
        blocks = new List<Tile>();
    }
    public bool Create(ref AStarSearch search, BattleVector2Int start, BattleVector2Int end)
    {
        points = search.Find(new Vector2Int(start.x, start.y), new Vector2Int(end.x, end.y)).ToList();
        if (!IsValid(ref points, new AStarPathfinding.Vector2Int(start.x, start.y), new AStarPathfinding.Vector2Int(end.x, end.y)))
        {
            points = null;
            return false;
        }
        else
        {
            this.start.x = start.x;
            this.start.y = start.y;
            this.end.x = end.x;
            this.end.y = end.y;
            return true;
        }
    }
    public static bool IsValid(ref List<Cell> points, AStarPathfinding.Vector2Int start, AStarPathfinding.Vector2Int end)
    {
        if (!points.Any() || !points.Last().Location.Equals(end) || !points.First().Location.Equals(start))
        {
            return false;
        }
        return true;
    }
    public BattleVector2Int start;
    public BattleVector2Int end;
    public List<Cell> points = null;
    public float length = 0;
    public List<Tile> blocks = null;
    // public float blocksHealth = 0;
}
