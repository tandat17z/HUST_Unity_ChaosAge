using System;
using System.Collections.Generic;
using ChaosAge.Config;
using ChaosAge.input;
using ChaosAge.UI.elements;
using DatSystem;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop : Panel
{
    [SerializeField]
    Button btnClose;

    [Header("Building In Shop")]
    [SerializeField]
    private UIBuildingInShop[] _buildingInShops;

    public override void OnSetup()
    {
        base.OnSetup();
        btnClose.onClick.AddListener(Close);

        foreach (var buildingInShop in _buildingInShops)
        {
            buildingInShop.OnSetup();
        }
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
    }

    public override void Close()
    {
        // InputHandler.Instance.ActiveInteract(true);
        base.Close();
    }
}
