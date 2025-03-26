using ChaosAge.Data;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;

public class UIBattle : Panel
{
    public override void OnSetup()
    {
        base.OnSetup();
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int x = 0;
            int y = 0;
            BattleManager.Instance.AddUnit(EUnitType.barbarian, x, y);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int x = 0;
            int y = 0;
            BattleManager.Instance.AddUnit(EUnitType.archer, x, y);
        }
    }
}
