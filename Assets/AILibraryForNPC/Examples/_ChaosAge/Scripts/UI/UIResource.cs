using ChaosAge.data;
using DatSystem;
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

    public void SetInfo(int amount)
    {
        var _maxAmount = DataManager.Instance.GetMaxResource(ResourceType);
        _textMeshProUGUI.text = amount.ToString() + "/" + _maxAmount;
        _imageSlider.fillAmount = (float)amount / _maxAmount;
    }
}

[System.Serializable]
public enum EResourceType
{
    Gold,
    Elixir,
}
