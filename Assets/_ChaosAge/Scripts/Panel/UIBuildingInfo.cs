using System.Collections;
using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.manager;
using DatSystem.UI;
using TMPro;
using UnityEngine;

public class UIBuildingInfo : Panel
{
    [SerializeField]
    RectTransform pInfo;

    [SerializeField]
    TMP_Text textName;

    [SerializeField]
    TMP_Text textLevel;

    public override void OnSetup()
    {
        base.OnSetup();
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
    }

    private void Update()
    {
        var building = BuildingManager.Instance.SelectedBuilding;
        if (building)
        {
            var screenPos = Camera.main.WorldToScreenPoint(building.transform.position);
            pInfo.anchoredPosition = new Vector2(
                screenPos.x - Screen.width / 2,
                screenPos.y - Screen.height / 2
            );

            ShowTextInfo(building);
        }
    }

    private void ShowTextInfo(Building0 building)
    {
        textName.text = building.Type.ToString();
        textLevel.text = $"Level {building.Level}";
    }
}
