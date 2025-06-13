using System;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PopupLoss : Panel
{
    [SerializeField]
    Button btnClose;

    [SerializeField]
    Button btnReplay;

    public override void OnSetup()
    {
        base.OnSetup();

        btnClose.onClick.AddListener(OnClose);
        btnReplay.onClick.AddListener(OnReplay);
    }

    private void OnClose()
    {
        Close();
        GameManager.Instance.SwitchToCity();
    }

    private void OnReplay()
    {
        Close();
        GameManager.Instance.SwitchToBattleAI();
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        // InputHandler.Instance.ActiveInteract(false);
    }
}
