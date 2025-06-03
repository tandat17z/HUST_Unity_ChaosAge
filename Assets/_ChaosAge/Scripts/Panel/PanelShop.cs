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
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

        Debug.LogWarning("Open");

        foreach (var buildingInShop in _buildingInShops)
        {
            buildingInShop.SetInfo();
        }
    }

    public override void Close()
    {
        // InputHandler.Instance.ActiveInteract(true);
        base.Close();
    }
}
