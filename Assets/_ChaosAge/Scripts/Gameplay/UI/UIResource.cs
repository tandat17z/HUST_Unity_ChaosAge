using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResource : MonoBehaviour
{
    public EResourceType ResourceType;

    [SerializeField]
    private TextMeshProUGUI _textMeshProUGUI;

    [SerializeField]
    private Image _imageSlider;

    private int _maxAmount = 100;

    public void SetInfo(int amount)
    {
        _textMeshProUGUI.text = amount.ToString();
        _imageSlider.fillAmount = (float)amount / _maxAmount;
    }
}

[System.Serializable]
public enum EResourceType
{
    Gold,
    Elixir,
}
