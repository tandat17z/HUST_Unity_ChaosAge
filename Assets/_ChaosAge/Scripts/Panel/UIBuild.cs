using System.Net.NetworkInformation;
using ChaosAge.Config;
using ChaosAge.Data;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIBuild : Panel
{
    [SerializeField] RectTransform pButton;
    [SerializeField] Button btnConfirm;
    [SerializeField] Button btnCancel;

    private PlayerData _playerData;
    private GameConfig _gameConfig;

    public override void OnSetup()
    {
        base.OnSetup();
        btnConfirm.onClick.AddListener(OnConfirm);
        btnCancel.onClick.AddListener(OnCancel);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

        _playerData = DataManager.Instance.PlayerData;
        _gameConfig = DataManager.Instance.GameConfig;
    }

    private void OnConfirm()
    {
        var building = BuildingManager.Instance.SelectedBuilding;
        //var buildingConfig = _gameConfig.GetBuildingConfig(building.GetData().type);
        if (building)
        {
            //if (_playerData.Gold >= buildingConfig.requireGold && _playerData.Elixir >= buildingConfig.requireElixir && _playerData.Gem >= buildingConfig.requireGem)
            {
                var type = building.GetData().type;
                var buildingData = new BuildingData(type);

                BuildingManager.Instance.AddListBuilding(building);
                BuildingManager.Instance.Unselect();
                Debug.Log("Build successful");
                Close();

            }
        }
    }

    private void OnCancel()
    {
        var building = BuildingManager.Instance.SelectedBuilding;
        if (building)
        {
            BuildingManager.Instance.Unselect();
            building.RemovedFromGrid();
            Close();
        }
    }

    private void Update()
    {
        var building = BuildingManager.Instance.SelectedBuilding;
        if (building)
        {
            var screenPos = Camera.main.WorldToScreenPoint(building.transform.position);
            pButton.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);
        }
    }
}
