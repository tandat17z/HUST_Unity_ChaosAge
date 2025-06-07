using System;
using ChaosAge.data;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonUnit : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private EUnitType _unitType;

    [SerializeField]
    Button btn;

    [SerializeField]
    Image imgDeselect;

    [SerializeField]
    TMP_Text txtName;

    [SerializeField]
    TMP_Text txtLevel;

    [SerializeField]
    private int _num;

    private void Start() { }

    public void SetUnit(EUnitType unitType)
    {
        _unitType = unitType;
        _num = DataManager.Instance.PlayerData.GetUnitNum(unitType);
        txtName.text = unitType.ToString();
        txtLevel.text = _num.ToString();

        Deselect();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Select();
    }

    public void Select()
    {
        GameManager.Instance.Log($"Select {_unitType}");
        var panel = PanelManager.Instance.GetPanel<PanelBattle>();
        panel.SetSelectedUnit(_unitType);
        imgDeselect.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        imgDeselect.gameObject.SetActive(false);
    }
}
