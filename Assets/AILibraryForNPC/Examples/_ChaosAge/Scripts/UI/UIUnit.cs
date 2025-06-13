using ChaosAge.data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnit : MonoBehaviour
{
    public EUnitType UnitType;

    [SerializeField]
    private TMP_Text _textName;
    [SerializeField]
    private TMP_Text _textAmount;

    public void SetInfo(int amount)
    {
        _textName.text = UnitType.ToString();
        _textAmount.text = amount.ToString();
    }
}
