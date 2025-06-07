using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PopupSelectLevel : Panel
{
    [SerializeField]
    Button btnClose;

    [SerializeField]
    private UIButtonLevel[] _buttons;

    public override void OnSetup()
    {
        base.OnSetup();

        btnClose.onClick.AddListener(Close);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].SetLevel(i + 1);
        }
    }
}
