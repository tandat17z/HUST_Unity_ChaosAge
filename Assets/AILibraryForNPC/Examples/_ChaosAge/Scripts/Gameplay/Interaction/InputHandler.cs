﻿namespace ChaosAge.interaction
{
    using ChaosAge.AI.battle;
    using ChaosAge.camera;
    using ChaosAge.manager;
    using DatSystem.UI;
    using DatSystem.utils;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class InputHandler : Singleton<InputHandler>
    {
        public enum EInputStatus
        {
            BuildingMoving,
            CameraMoving,
        }

        protected override void OnAwake() { }

        private CameraController cameraController;

        private bool isDragging = false;
        private Vector2 initialDragPosition;
        private Vector2 currentDragPosition;
        private float startTouchTime;

        private float initialTouchDistance;
        private bool isPinching = false;
        private Plane groundPlane;
        private float zoomSpeed = 5f;

        private EInputStatus _inputStatus;

        private void Start()
        {
            groundPlane = new Plane(Vector3.up, Vector3.zero);
            cameraController = Camera.main.GetComponent<CameraController>();
            _inputStatus = EInputStatus.CameraMoving;
        }

        #region Handle Touch

        private void Update()
        {
            if (PanelManager.Instance.GetPanel<PanelShop>() != null)
                return;

            HandleTouchInput();
            HandleZoomInput();
        }

        private void HandleZoomInput()
        {
            // Handle mouse wheel zoom
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                HandlePinchZoom(-scrollInput * zoomSpeed);
            }
        }

        private void HandleTouchInput()
        {
            // if (Input.touchCount == 1)
            // {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Debug.Log("Click lên UI");
                return;
            }
            HandleOneTouch();
            // }

            if (Input.touchCount == 2)
            {
                HandleTwoTouch();
            }
        }

        private void HandleOneTouch()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                initialDragPosition = Input.mousePosition;
                startTouchTime = Time.time;
                HandleTouchBegin();
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                currentDragPosition = Input.mousePosition;
                HandleTouchDrag(initialDragPosition, currentDragPosition);
                initialDragPosition = currentDragPosition;
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false;

                if (Time.time - startTouchTime < 0.2f)
                {
                    HandleTap();
                }

                HandleTouchEnd();
            }
        }

        private void HandleTwoTouch()
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began)
            {
                initialTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                isPinching = true;
            }
            else if (
                isPinching && (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            )
            {
                float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                float deltaDistance = currentTouchDistance - initialTouchDistance;
                HandlePinchZoom(deltaDistance);
                initialTouchDistance = currentTouchDistance;
            }
            else if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
            {
                isPinching = false;
            }
        }

        #endregion

        private void HandleTouchBegin()
        {
            var cellPos = GetCellPosition(Input.mousePosition);
            var building = BuildingManager.Instance.SelectedBuilding;

            if (building != null && building.IsCellPositionInBuilding(cellPos))
            {
                _inputStatus = EInputStatus.BuildingMoving;
                building.StartMoving(cellPos);

                var panelMainUI = PanelManager.Instance.GetPanel<PanelMainUI>();
                panelMainUI.HideUI();


                if (PanelManager.Instance.GetPanel<PanelBuildingOption>() != null)
                {
                    PanelManager.Instance.ClosePanel<PanelBuildingOption>();
                }
            }
            else
            {
                _inputStatus = EInputStatus.CameraMoving;
            }
        }

        private void HandleTouchDrag(Vector2 start, Vector2 end)
        {
            if (_inputStatus == EInputStatus.CameraMoving)
            {
                // TODO: Implement logic for dragging, e.g., moving the camera or buildings
                Vector3 worldStart = GetWorldPosition(start);
                Vector3 worldEnd = GetWorldPosition(end);

                Vector3 worldDelta = worldEnd - worldStart;
                cameraController.Move(new Vector2(worldDelta.x, worldDelta.z));
            }
            else if (_inputStatus == EInputStatus.BuildingMoving)
            {
                if (GameManager.Instance.GameState == GameState.BattleAI)
                    return;
                // TODO: Implement logic for moving buildings
                var targetCellPos = GetCellPosition(end);
                var selectedBuilding = BuildingManager.Instance.SelectedBuilding;
                selectedBuilding.MoveTo(targetCellPos);
            }
        }

        private void HandleTouchEnd()
        {
            var selectedBuilding = BuildingManager.Instance.SelectedBuilding;
            if (selectedBuilding != null)
            {
                if (PanelManager.Instance.GetPanel<PanelBuildingOption>() == null)
                {
                    if (GameManager.Instance.GameState == GameState.BattleAI)
                        return;

                    if(selectedBuilding.Level == 0) return;

                }
            }
            else
            {
                if (PanelManager.Instance.GetPanel<PanelBuildingOption>() != null)
                {
                    PanelManager.Instance.ClosePanel<PanelBuildingOption>();
                }
            }

            var panelMainUI = PanelManager.Instance.GetPanel<PanelMainUI>();
            if(panelMainUI != null){
                panelMainUI.ShowUI();
            }
        }

        private void HandleTap()
        {
            var cellPos = GetCellPosition(Input.mousePosition);
            if (GameManager.Instance.GameState == GameState.BattleAI)
            {
                var unitType = PanelBattle.SelectedUnit;
                AIBattleManager.Instance.TryAddUnit(unitType, cellPos);
                return;
            }
            else{
                var selectedBuilding = BuildingManager.Instance.SelectedBuilding;
                if (selectedBuilding != null)
                {
                    selectedBuilding.StopMoving();
                }
            }

            var building = BuildingManager.Instance.SelectBuilding(cellPos);
            if (building != null) {
                if(PanelManager.Instance.GetPanel<PanelBuildingOption>() != null){
                    PanelManager.Instance.ClosePanel<PanelBuildingOption>();
                }
                DOVirtual.DelayedCall(0.25f, () =>
                {
                    PanelManager.Instance.OpenPanel<PanelBuildingOption>();
                });
            }
            else
            {
                BuildingManager.Instance.DeselectBuilding();
                // inputStatus = EInputStatus.CameraMoving;
                Debug.Log("No building selected");
            }
        }

        private void HandlePinchZoom(float delta)
        {
            cameraController.Zoom(delta);
        }

        private Vector2 GetCellPosition(Vector2 screenPosition)
        {
            var pos = GetWorldPosition(screenPosition);
            return BuildingManager.Instance.Grid.GetCellPosition(pos);
        }

        private Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
            return Vector3.zero;
        }
    }
}
