using ChaosAge.building;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelBuildingOption : Panel
{
    [SerializeField]
    private Button btnOptionInfo;

    [Header("Upgrade")]
    [SerializeField]
    private Button btnOptionUpgrade;

    [SerializeField]
    private UIResourceCost uiResourceCost;

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
        if (BuildingManager.Instance.CanUpgradeBuilding(_building.Type, _building.Level + 1))
        {
            BuildingManager.Instance.BeginUpgrade(_building);
        }
        else
        {
            GameManager.Instance.Log("Cannot upgrade building");
        }
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

        _building = BuildingManager.Instance.SelectedBuilding;

        var buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>(
            $"{_building.Type}_{_building.Level + 1}"
        );
        if (buildingConfigSO != null)
        {
            uiResourceCost.SetInfo(buildingConfigSO.costs.ToArray());
        }
    }

    public override void Close()
    {
        base.Close();
    }
}
