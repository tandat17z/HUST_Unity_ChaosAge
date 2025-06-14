using ChaosAge.data;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelMainUI : Panel
{
    [Header("Resources")]
    [SerializeField]
    UIResource[] UIResources;

    [SerializeField]
    UIUnit[] UIUnits;

    [Header("Buttons")]
    [SerializeField]
    Button btnShop;

    [SerializeField]
    Button btnBattle;

    [Header("UI")]
    [SerializeField]
    GameObject rootDr;
    [SerializeField]
    GameObject rootDl;
    [SerializeField]
    GameObject rootTr;

    private PlayerData _playerData;

    #region setup
    public override void OnSetup()
    {
        base.OnSetup();

        btnShop.onClick.AddListener(ShopButtonClicked);
        btnBattle.onClick.AddListener(BattleButtonClicked);
    }

    private void ShopButtonClicked()
    {
        PanelManager.Instance.OpenPanel<PanelShop>();

        // InputHandler.Instance.ActiveInteract(false);
    }

    private void BattleButtonClicked()
    {
        PanelManager.Instance.OpenPanel<PopupSelectLevel>();

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

    private void UpdateUIUnit()
    {
        foreach (var unit in UIUnits)
        {
            unit.SetInfo(_playerData.GetUnitNum(unit.UnitType));
        }
    }

    private void Update()
    {
        UpdateUIResource();
        UpdateUIUnit();
    }

    public void ShowUI(){
        var rectDr = rootDr.GetComponent<RectTransform>();
        rectDr.DOKill();
        rectDr.DOAnchorPos(Vector2.zero, 0.2f).SetEase(Ease.OutBack);

        var rectTr = rootTr.GetComponent<RectTransform>();
        rectTr.DOKill();
        rectTr.DOAnchorPos(Vector2.zero, 0.2f).SetEase(Ease.OutBack);

        var rectDl = rootDl.GetComponent<RectTransform>();
        rectDl.DOKill();
        rectDl.DOAnchorPos(Vector2.zero, 0.2f).SetEase(Ease.OutBack);
    }

    public void HideUI(){
        var rectDr = rootDr.GetComponent<RectTransform>();
        rectDr.DOKill();
        rectDr.DOAnchorPos(new Vector2(450, 0), 0.2f).SetEase(Ease.InBack);

        var rectTr = rootTr.GetComponent<RectTransform>();
        rectTr.DOKill();
        rectTr.DOAnchorPos(new Vector2(450, 0), 0.2f).SetEase(Ease.InBack);

        var rectDl = rootDl.GetComponent<RectTransform>();
        rectDl.DOKill();
        rectDl.DOAnchorPos(new Vector2(-450, 0), 0.2f).SetEase(Ease.InBack);
    }
}
