using System;
using System.Collections.Generic;
using ChaosAge.Config;
using ChaosAge.input;
using ChaosAge.UI.elements;
using DatSystem;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop : Panel
{
    [SerializeField]
    Button btnClose;

    [Header("")]
    [SerializeField]
    Transform container;

    [SerializeField]
    Transform buttonBuildPrefab;

    private List<ButtonBuild> _buttonBuilds;

    public override void OnSetup()
    {
        base.OnSetup();
        btnClose.onClick.AddListener(Close);

        _buttonBuilds = new();
        foreach (EBuildingType type in Enum.GetValues(typeof(EBuildingType)))
        {
            var btnBuild = Instantiate(buttonBuildPrefab, container).GetComponent<ButtonBuild>();
            btnBuild.SetInfo(type);

            _buttonBuilds.Add(btnBuild);
        }
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

        var playerData = DataManager.Instance.PlayerData;
        var gameConfig = DataManager.Instance.GameConfig;
        foreach (var btn in _buttonBuilds)
        {
            var type = btn.buildingType;
            btn.SetInteractable(
                playerData.GetBuildingNumber(type) < gameConfig.GetBuildingMaxNumber(type)
            );
        }
    }

    public override void Close()
    {
        // InputHandler.Instance.ActiveInteract(true);
        base.Close();
    }
}
