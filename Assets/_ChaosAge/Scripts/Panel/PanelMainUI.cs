using ChaosAge.Data;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelMainUI : Panel
{
    [Header("Resources")]
    [SerializeField]
    UIResource[] UIResources;

    [Header("Buttons")]
    [SerializeField]
    Button btnShop;

    [SerializeField]
    Button btnBattle;

    private PlayerData _playerData;

    #region setup
    public override void OnSetup()
    {
        base.OnSetup();

        btnShop.onClick.AddListener(ShopButtonClicked);
        btnBattle.onClick.AddListener(BattleButtonClicked);
    }

    private void BattleAIButtonClicked()
    {
        GameManager.Instance.SwitchToBattleAI();
    }

    private void ShopButtonClicked()
    {
        PanelManager.Instance.OpenPanel<PanelShop>();

        // InputHandler.Instance.ActiveInteract(false);
    }

    private void BattleButtonClicked()
    {
        GameManager.Instance.SwitchToBattle();

        // InputHandler.Instance.ActiveInteract(false);
    }
    #endregion

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        _playerData = DataManager.Instance.PlayerData;
        UpdateUIResource();
    }

    private void UpdateUIResource()
    {
        foreach (var resource in UIResources)
        {
            resource.SetInfo(_playerData.GetResource(resource.ResourceType));
        }
    }

    private void Update()
    {
        UpdateUIResource();
    }
}
