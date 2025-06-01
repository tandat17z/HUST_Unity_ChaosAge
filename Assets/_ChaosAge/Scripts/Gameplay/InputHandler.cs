namespace ChaosAge
{
    using System;
    using ChaosAge.camera;
    using DatSystem.utils;
    using UnityEngine;

    public class InputHandler : Singleton<InputHandler>
    {
        protected override void OnAwake() { }

        private CameraController cameraController;

        private bool isDragging = false;
        private Vector2 initialDragPosition;
        private Vector2 currentDragPosition;

        private float initialTouchDistance;
        private bool isPinching = false;

        private Plane groundPlane;

        private float zoomSpeed = 5f;

        private void Start()
        {
            groundPlane = new Plane(Vector3.up, Vector3.zero);
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        #region Handle Touch

        private void Update()
        {
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
            // throw new NotImplementedException();
        }

        private void HandleTouchDrag(Vector2 start, Vector2 end)
        {
            Vector2 delta = end - start;
            // TODO: Implement logic for dragging, e.g., moving the camera or buildings
            Vector3 worldStart = GetWorldPosition(start);
            Vector3 worldEnd = GetWorldPosition(end);

            Vector3 worldDelta = worldEnd - worldStart;
            cameraController.Move(new Vector2(worldDelta.x, worldDelta.z));
        }

        private void HandleTouchEnd()
        {
            var cellPos = GetCellPosition(Input.mousePosition);
            var building = BuildingManager.Instance.SelectBuilding(cellPos);
            if (building != null)
            {
                Debug.Log("Building selected: " + building.Type);
            }
            else
            {
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
