using System;

public struct BattleVector2
{
    public float x;
    public float y;

    public BattleVector2(float x, float y) { this.x = x; this.y = y; }

    public static BattleVector2 LerpUnclamped(BattleVector2 a, BattleVector2 b, float t)
    {
        return new BattleVector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
    }

    public static float Distance(BattleVector2 a, BattleVector2 b)
    {
        float diff_x = a.x - b.x;
        float diff_y = a.y - b.y;
        return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
    }

    public static float Distance(BattleVector2Int a, BattleVector2Int b)
    {
        return Distance(new BattleVector2(a.x, a.y), new BattleVector2(b.x, b.y));
    }

    /// <summary>
    /// Smootly moves a vector2 to another vector2 with desired speed.
    /// </summary>
    /// <param name="source">Position which you want to move from.</param>
    /// <param name="target">Position which you want to reach.</param>
    /// <param name="speed">Move distance per second. Note: Do not multiply delta time to speed.</param>
    public static BattleVector2 LerpStatic(BattleVector2 source, BattleVector2 target, float deltaTime, float speed)
    {
        if ((source.x == target.x && source.y == target.y) || speed <= 0) { return source; }
        float distance = Distance(source, target);
        float t = speed * deltaTime;
        if (t > distance) { t = distance; }
        return LerpUnclamped(source, target, distance == 0 ? 1 : t / distance);
    }

    public static BattleVector2 GridToWorldPosition(BattleVector2Int position) // ok
    {
        return new BattleVector2(position.x * ConfigData.gridSize + ConfigData.gridSize / 2f, position.y * ConfigData.gridSize + ConfigData.gridSize / 2f);
    }
}