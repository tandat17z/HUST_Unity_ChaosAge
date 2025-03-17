using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop : Panel
{
    [SerializeField] Button btnClose;

    public override void OnSetup()
    {
        base.OnSetup();
        btnClose.onClick.AddListener(CloseShop);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

    }

    private void CloseShop()
    {

    }

}
