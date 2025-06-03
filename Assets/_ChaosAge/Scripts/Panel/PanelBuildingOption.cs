using System;
using ChaosAge;
using ChaosAge.manager;
using DatSystem.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelBuildingOption : Panel
{
    [SerializeField]
    private Button btnOptionUpgrade;

    [SerializeField]
    private Button btnOptionInfo;

    private Building _building;

    public override void OnSetup()
    {
        base.OnSetup();
        btnOptionUpgrade.onClick.AddListener(OnClickOptionUpgrade);
        btnOptionInfo.onClick.AddListener(OnClickOptionInfo);
    }

    private void OnClickOptionInfo()
    {
        GameManager.Instance.Log($"Building: {_building.name} Lv.{_building.Level}");
    }

    private void OnClickOptionUpgrade()
    {
        if (ChaosAge.BuildingManager.Instance.CanUpgradeBuilding(_building.Type, _building.Level))
        {
            _building.Upgrade();
        }
        else
        {
            GameManager.Instance.Log("Cannot upgrade building");
        }
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

        Debug.Log("Open PanelBuildingOption");
        _building = ChaosAge.BuildingManager.Instance.SelectedBuilding;
    }

    public override void Close()
    {
        base.Close();
    }
}
