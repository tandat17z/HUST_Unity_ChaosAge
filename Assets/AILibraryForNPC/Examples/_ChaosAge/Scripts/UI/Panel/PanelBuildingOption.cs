using ChaosAge.building;
using ChaosAge.manager;
using DatSystem.UI;
using DG.Tweening;
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
    private Tween _tweenClose;

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
        if (
            _building.CheckUpgrading() == false
            && BuildingManager.Instance.CanUpgradeBuilding(_building.Type, _building.Level)
        )
        {
            BuildingManager.Instance.StartUpgrade(_building);
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

        EffectOpen();
    }

    public override void Close()
    {
        EffectClose();
        _tweenClose?.Kill();
        _tweenClose = DOVirtual.DelayedCall(0.25f, () =>
        {
            base.Close();
        });
    }

    private void EffectOpen()
    {
        var rectInfo = btnOptionInfo.GetComponent<RectTransform>();
        rectInfo.DOKill();
        var pos0 = rectInfo.anchoredPosition;
        pos0.y = -125;
        rectInfo.anchoredPosition = pos0;
        rectInfo.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack);

        var rectUpgrade = btnOptionUpgrade.GetComponent<RectTransform>();
        rectUpgrade.DOKill();
        var pos1 = rectUpgrade.anchoredPosition;
        pos1.y = -125;
        rectUpgrade.anchoredPosition = pos1;
        rectUpgrade.DOAnchorPosY(0, 0.3f).SetDelay(0.2f).SetEase(Ease.OutBack);
    }

    private void EffectClose()
    {
        var rectInfo = btnOptionInfo.GetComponent<RectTransform>();
        rectInfo.DOKill();
        var pos0 = rectInfo.anchoredPosition;
        pos0.y = 0;
        rectInfo.anchoredPosition = pos0;
        rectInfo.DOAnchorPosY(-125, 0.15f).SetEase(Ease.InBack);

        var rectUpgrade = btnOptionUpgrade.GetComponent<RectTransform>();
        rectUpgrade.DOKill();
        var pos1 = rectUpgrade.anchoredPosition;
        pos1.y = 0;
        rectUpgrade.anchoredPosition = pos1;
        rectUpgrade.DOAnchorPosY(-125, 0.15f).SetDelay(0.1f).SetEase(Ease.InBack);

    }
}
