using System;
using System.Collections.Generic;
using ChaosAge;
using ChaosAge.Config;
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
    private List<GroupResource> _groupResources;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void SetInfo()
    {
        var playerData = DataManager.Instance.PlayerData;
        var buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>($"{_buildingType}_{1}");
        _textName.text = _buildingType.ToString();

        var townhallConfig =
            ChaosAge.BuildingManager.Instance.TownHall.BuildingConfigSO as TownhallConfigSO;
        var num = ChaosAge.BuildingManager.Instance.GetBuildingNumber(_buildingType);
        var maxNum = townhallConfig.GetLimitBuildingNumber(_buildingType);
        _textNumber.text = $"{num}/{maxNum}";
        _textNumber.color = num >= maxNum ? Color.red : Color.white;

        ResetGroupResource();
        foreach (var cost in buildingConfigSO.costs)
        {
            var groupResource = GetGroupResource(cost.resourceType);
            groupResource.textCost.text = cost.quantity.ToString();
            groupResource.textCost.color =
                cost.quantity >= playerData.GetResource(cost.resourceType)
                    ? Color.red
                    : Color.white;
            groupResource.objCost.SetActive(true);
        }
    }

    private void ResetGroupResource()
    {
        foreach (var groupResource in _groupResources)
        {
            groupResource.objCost.SetActive(false);
        }
    }

    private GroupResource GetGroupResource(EResourceType resourceType)
    {
        return _groupResources.Find(group => group.resourceType == resourceType);
    }

    private void OnClick()
    {
        if (ChaosAge.BuildingManager.Instance.CanUpgradeBuilding(_buildingType, 1))
        {
            ChaosAge.BuildingManager.Instance.CreateBuilding(_buildingType);
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

    [System.Serializable]
    public class GroupResource
    {
        public EResourceType resourceType;
        public GameObject objCost;
        public TMP_Text textCost;
    }
}
