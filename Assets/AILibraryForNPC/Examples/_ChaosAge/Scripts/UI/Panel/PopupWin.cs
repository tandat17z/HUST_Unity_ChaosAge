using System;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : Panel
{
    [SerializeField]
    Button btnClose;

    [SerializeField]
    Button btnReplay;

    public override void OnSetup()
    {
        base.OnSetup();

        btnClose.onClick.AddListener(Close);
        btnReplay.onClick.AddListener(Replay);
    }

    private void Replay()
    {
        GameManager.Instance.SwitchToBattleAI();
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        // InputHandler.Instance.ActiveInteract(false);
    }

    public override void Close()
    {
        // InputHandler.Instance.ActiveInteract(true);

        base.Close();
        GameManager.Instance.SwitchToCity();
    }
}
