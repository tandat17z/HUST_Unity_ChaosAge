using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonClaim : MonoBehaviour
{
    [SerializeField]
    private Image _iconGold;

    [SerializeField]
    private Image _iconElixir;

    public void SetIcon(EResourceType resourceType)
    {
        _iconGold.gameObject.SetActive(resourceType == EResourceType.Gold);
        _iconElixir.gameObject.SetActive(resourceType != EResourceType.Gold);
    }
}
