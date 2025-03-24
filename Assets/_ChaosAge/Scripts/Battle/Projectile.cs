public class Projectile
{
    public int target = -1;
    public float damage = 0;
    public float splash = 0;
    public float timer = 0;
    public TargetType type = TargetType.unit;
    public bool heal = false;
}

public enum TargetType
{
    unit, building
}