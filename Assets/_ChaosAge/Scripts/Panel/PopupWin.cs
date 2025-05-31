using System;
using System.Collections.Generic;
using ChaosAge.Config;
using ChaosAge.input;
using ChaosAge.manager;
using ChaosAge.UI.elements;
using DatSystem;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : Panel
{
    [SerializeField]
    Button btnClose;

    public override void OnSetup()
    {
        base.OnSetup();

        btnClose.onClick.AddListener(Close);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        // InputHandler.Instance.ActiveInteract(false);
    }

    public override void Close()
    {
        // InputHandler.Instance.ActiveInteract(true);

        GameManager.Instance.SwitchToCity();

        PanelManager.Instance.ClosePanel<PanelBattle>();
        base.Close();
    }
}
