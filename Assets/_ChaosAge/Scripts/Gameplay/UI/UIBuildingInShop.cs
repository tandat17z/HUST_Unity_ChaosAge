using ChaosAge;
using ChaosAge.Config;
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
    private TMP_Text _textCost;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        BuildingManager.Instance.CreateBuilding(_buildingType);
        PanelManager.Instance.ClosePanel<PanelShop>();
    }

    public void OnSetup()
    {
        _textName.text = _buildingType.ToString();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        OnSetup();
    }
#endif
}
