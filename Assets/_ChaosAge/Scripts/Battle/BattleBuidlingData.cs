using ChaosAge.Data;
using System.Collections.Generic;
using static ChaosAge.Battle.Battle;

public class BattleBuildingData
{
    public BuildingData building = null;
    public float health = 0;
    public int target = -1;
    public double attackTimer = 0;
    public float percentage = 0;
    public BattleVector2 worldCenterPosition;
    public AttackCallback attackCallback = null;
    public FloatCallback destroyCallback = null;
    public FloatCallback damageCallback = null;


    public void Initialize()
    {
        health = building.health;
        percentage = building.percentage;
    }

    public void TakeDamage(float damage, ref AStarPathfinding.Grid grid, ref List<Tile> blockedTiles, ref float percentage)
    {
        if (health <= 0) { return; }
        health -= damage;
        if (damageCallback != null)
        {
            damageCallback.Invoke((long)building.type, damage);
        }


        // die
        if (health < 0) { health = 0; }
        if (health <= 0)
        {
            for (int x = building.x; x < building.x + building.columns; x++)
            {
                for (int y = building.y; y < building.y + building.rows; y++)
                {
                    grid[x, y].Blocked = false;
                    for (int i = 0; i < blockedTiles.Count; i++)
                    {
                        if (blockedTiles[i].position.x == x && blockedTiles[i].position.y == y)
                        {
                            blockedTiles.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (this.percentage > 0)
            {
                percentage += this.percentage;
            }
            if (destroyCallback != null)
            {
                destroyCallback.Invoke((long)building.type, this.percentage);
            }
        }
    }
}
