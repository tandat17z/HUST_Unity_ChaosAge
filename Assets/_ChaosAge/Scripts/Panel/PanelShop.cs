using ChaosAge.input;
using ChaosAge.manager;
using ChaosAge.UI.elements;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop : Panel
{
    [SerializeField] Button btnClose;
    [SerializeField] ButtonBuild uiBuilding;

    public override void OnSetup()
    {
        base.OnSetup();
        btnClose.onClick.AddListener(Close);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

    }
    public override void Close()
    {
        InputHandler.Instance.ActiveInteract(true);
        base.Close();

    }
}
