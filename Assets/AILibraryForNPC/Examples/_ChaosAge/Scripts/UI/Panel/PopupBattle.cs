using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PopupBattle : Panel
{
    [SerializeField]
    Button btnClose;

    [SerializeField]
    Button[] btnLevels;

    public override void OnSetup()
    {
        base.OnSetup();

        btnClose.onClick.AddListener(Close);

        int i = 1;
        foreach (var btn in btnLevels)
        {
            btn.onClick.AddListener(() => {
                // AIBattleManager.Instance.LoadLevel(i);
            });
            i += 1;
        }
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
    }
}
