using UnityEngine;

public class Attack : GAction
{
    [SerializeField]
    private bool isBuildingDied = false;

    public override bool IsActionComplete()
    {
        return isBuildingDied == true;
    }

    public override void Perform()
    {
        Debug.Log("Attack");
    }

    public override void PostPerform() { }

    public override bool PrePerform()
    {
        isBuildingDied = false;
        return true;
    }

    protected override void OnAwake() { }
}
