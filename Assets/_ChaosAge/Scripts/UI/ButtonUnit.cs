using System.Collections;
using System.Collections.Generic;
using ChaosAge.Data;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUnit : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] EUnitType type;
    [SerializeField] Image image;


    public EUnitType Type { get { return type; } }
    private bool _selected;

    private void Awake()
    {
        btn.onClick.AddListener(OnButtonUnit);
        _selected = false;
    }

    private void OnButtonUnit()
    {
        if (PanelBattle.SelectedButtonUnit != null && PanelBattle.SelectedButtonUnit != this)
        {
            PanelBattle.SelectedButtonUnit.SetSelected(false);
        }

        if (!_selected)
        {
            SetSelected(true);
            PanelBattle.SelectedButtonUnit = this;
        }
    }

    private void SetSelected(bool selected)
    {
        image.color = selected ? Color.green : Color.white;
    }
}
