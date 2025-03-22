using System.Collections;
using System.Collections.Generic;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIBuild : Panel
{
    [SerializeField] Button btnConfirm;
    [SerializeField] Button btnCancel;

    public override void OnSetup()
    {
        base.OnSetup();
        btnConfirm.onClick.AddListener(OnConfirm);
        btnCancel.onClick.AddListener(OnCancel);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);

    }

    private void OnConfirm()
    {

    }

    private void OnCancel()
    {
        var building = BuildingManager.Instance.SelectedBuilding;
        if (building)
        {
            BuildingManager.Instance.SelectBuilding(null);
            building.RemovedFromGrid();
            Close();
        }
    }

    private void Update()
    {
        var building = BuildingManager.Instance.SelectedBuilding;
        if (building)
        {
            Vector3 end = BuildingManager.Instance.Grid.GetStartPosition(building.CurrentX, building.CurrentY);

            var cameraController = BuildingManager.Instance.CameraController;
            Vector3 planeDownLeft = cameraController.CameraScreenPositionToPlanePosition(Vector2.zero);
            Vector3 planeTopRight = cameraController.CameraScreenPositionToPlanePosition(new Vector2(Screen.width, Screen.height));

            float w = planeTopRight.x - planeDownLeft.x;
            float h = planeTopRight.z - planeDownLeft.z;

            float endW = (end.x - planeDownLeft.x - w / 2);
            float endH = (end.z - planeDownLeft.z - h / 2);

            Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height);

            Vector2 confirmPoint = screenPoint;
            var rectTransformInConfirm = btnConfirm.GetComponent<RectTransform>();
            confirmPoint.x -= (rectTransformInConfirm.rect.width + 10f);
            rectTransformInConfirm.anchoredPosition = confirmPoint;

            Vector2 cancelPoint = screenPoint;
            var rectTransformInCancel = btnCancel.GetComponent<RectTransform>();
            cancelPoint.x += (rectTransformInCancel.rect.width + 10f);
            rectTransformInCancel.anchoredPosition = cancelPoint;
        }
    }
}
