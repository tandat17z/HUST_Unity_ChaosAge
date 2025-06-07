using DatSystem.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PanelLog : Panel
{
    [SerializeField]
    private TMP_Text textLog;

    [SerializeField]
    private AnimationCurve animationCurve;

    public override void OnSetup()
    {
        base.OnSetup();
        Reset();
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
    }

    public override void Close()
    {
        base.Close();
    }

    public void ShowLog(string log)
    {
        textLog.text = log;
        textLog.gameObject.SetActive(true);
        var rect = textLog.GetComponent<RectTransform>();
        rect.DOKill();
        rect.DOAnchorPos(new Vector2(0, 300), 2f)
            .SetEase(animationCurve)
            .From(new Vector2(0, 0))
            .OnComplete(() =>
            {
                Reset();
            });
    }

    private void Reset()
    {
        textLog.gameObject.SetActive(false);
    }
}
