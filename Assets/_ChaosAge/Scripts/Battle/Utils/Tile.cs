
using ChaosAge.Config;

public class Tile
{
    public Tile(EBuildingType id, BattleVector2Int position, int index = -1)
    {
        this.id = id;
        this.position = position;
        this.index = index;
    }
    public EBuildingType id;
    public BattleVector2Int position;
    public int index = -1;
}