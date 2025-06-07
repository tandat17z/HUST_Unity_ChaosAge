using System;
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

        btnClose.onClick.AddListener(() =>
        {
            GameManager.Instance.SwitchToCity();
        });
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        for (int i = 0; i < btnUnits.Length; i++)
        {
            btnUnits[i].SetUnit((EUnitType)i);
        }
    }

    //private void Update()
    //{
    //    if (_isActive)
    //    {

    //        if (Input.GetMouseButtonDown(0) && SelectedButtonUnit != null)
    //        {
    //            var pos = InputHandler.Instance.GetPointerPositionInMap();
    //            var posCell = BuildingManager.Instance.Grid.ConvertGridPos(pos);
    //            BattleManager.Instance.AddUnit(SelectedButtonUnit.Type, (int)posCell.x, (int)posCell.y);
    //        }
    //    }
    //}

    public EUnitType GetCurrentBuildingType()
    {
        return SelectedUnit;
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
