using ChaosAge.data;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingInShop : MonoBehaviour
{
    [SerializeField]
    private EBuildingType _buildingType;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private Image _imageBuilding;

    [SerializeField]
    private TMP_Text _textName;

    [SerializeField]
    private TMP_Text _textNumber;

    [SerializeField]
    private UIResourceCost _uiResourceCost;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void SetInfo()
    {
        var playerData = DataManager.Instance.PlayerData;
        var buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>($"{_buildingType}_{1}");
        _textName.text = _buildingType.ToString();

        var townhallConfig = BuildingManager.Instance.TownHall.BuildingConfigSO as TownhallConfigSO;
        var num = BuildingManager.Instance.GetBuildingNumber(_buildingType);
        var maxNum = townhallConfig.GetLimitBuildingNumber(_buildingType);
        _textNumber.text = $"{num}/{maxNum}";
        _textNumber.color = num >= maxNum ? Color.red : Color.white;

        _uiResourceCost.SetInfo(buildingConfigSO.costs.ToArray());
    }

    private void OnClick()
    {
        if (BuildingManager.Instance.CanUpgradeBuilding(_buildingType, 1))
        {
            BuildingManager.Instance.CreateBuilding(_buildingType);
            PanelManager.Instance.ClosePanel<PanelShop>();
        }
        else
        {
            GameManager.Instance.Log("Cannot create building");
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _textName.text = _buildingType.ToString();
    }
#endif
}
