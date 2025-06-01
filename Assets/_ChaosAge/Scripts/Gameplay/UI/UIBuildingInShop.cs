using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.Config;
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
        Debug.Log("OnClick");
    }

    public void OnSetup()
    {
        _textName.text = _buildingType.ToString();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _textName.text = _buildingType.ToString();
    }
#endif
}
