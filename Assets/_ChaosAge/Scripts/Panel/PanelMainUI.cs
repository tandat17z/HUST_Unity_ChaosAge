using ChaosAge.Data;
using ChaosAge.input;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelMainUI : Panel
{
    [Header("Resources")]
    [SerializeField] TMP_Text textGold;
    [SerializeField] TMP_Text textElixir;
    [SerializeField] TMP_Text textGem;
    
    [Header("Buttons")]
    [SerializeField] Button btnShop;
    [SerializeField] Button btnBattle;

    private PlayerData _playerData;

    public override void OnSetup()
    {
        base.OnSetup();

        btnShop.onClick.AddListener(ShopButtonClicked);
        btnBattle.onClick.AddListener(BattleButtonClicked);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        _playerData = DataManager.Instance.PlayerData;

        InputHandler.Instance.ActiveInteract(true);
    }

    private void ShopButtonClicked()
    {
        PanelManager.Instance.OpenPanel<PanelShop>();

        InputHandler.Instance.ActiveInteract(false);
    }

    private void BattleButtonClicked()
    {
        GameManager.Instance.SwitchToBattle();

        InputHandler.Instance.ActiveInteract(false);
    }

    private void Update()
    {
        if (_playerData != null)
        {
            textGold.text = _playerData.Gold.ToString();
            textElixir.text = _playerData.Elixir.ToString();
            textGem.text = _playerData.Gem.ToString();
        }
    }
}
