using System;
using ChaosAge.AI.battle;
using ChaosAge.data;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelBattle : Panel
{
    [SerializeField]
    Button btnClose;

    [SerializeField]
    UIButtonUnit[] btnUnits;

    public static EUnitType SelectedUnit;

    public override void OnSetup()
    {
        base.OnSetup();

        btnClose.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        Close();
        AIBattleManager.Instance.SetResult(EGameState.Lose);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        for (int i = 0; i < btnUnits.Length; i++)
        {
            btnUnits[i].SetUnit((EUnitType)i);
        }
    }

    public void SetSelectedUnit(EUnitType unitType)
    {
        SelectedUnit = unitType;
        for (int i = 0; i < btnUnits.Length; i++)
        {
            btnUnits[i].Deselect();
        }
    }
}
