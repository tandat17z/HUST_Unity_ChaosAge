using AILibraryForNPC.GOAP;

public class Patient : GAgent
{
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);
    }
}
