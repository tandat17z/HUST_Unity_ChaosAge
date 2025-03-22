namespace ChaosAge.input
{
    using System;
    using ChaosAge.building;
    using ChaosAge.manager;
    using DatSystem.utils;
    using UnityEngine;

    public class InputHandler : Singleton<InputHandler>
    {
        protected override void OnAwake()
        {

        }




        public bool MoveMap { get => _moveMap; }
        public bool MoveBuilding { get => _moveBuilding; }

        private Controls _inputs;
        private bool _canInteract;
        private bool _moveMap;
        private bool _moveBuilding;

        private Building buildingWhenStartTouch;



        private void Awake()
        {
            _inputs = new Controls();
        }

        private void Start()
        {
            _canInteract = true;
        }

        private void OnEnable()
        {
            _inputs.Enable();


            // Nếu chạm vào công trình:
            // - Công trình này đã được chọn --> di chuyển 
            // - Chưa thì sẽ chọn nếu là touch
            // không thì sẽ move map
            _inputs.Main.Touch.started += _ => TouchStarted();
            _inputs.Main.Touch.canceled += _ => TouchCanceled();
        }

        private void OnDisable()
        {
            _inputs.Disable();

            _inputs.Main.Touch.started -= _ => TouchStarted();
            _inputs.Main.Touch.canceled -= _ => TouchCanceled();
        }

        public Vector2 GetMoveDelta()
        {
            return _inputs.Main.MoveDelta.ReadValue<Vector2>();
        }

        public Vector3 GetPointerPositionInMap()
        {
            var screenPos = _inputs.Main.PointerPosition.ReadValue<Vector2>();
            return ConvertScreenPositionToPlanePosition(screenPos);
        }

        public float GetMouseScroll()
        {
            return _inputs.Main.MouseScroll.ReadValue<float>();
        }

        private void TouchStarted()
        {
            Debug.Log("Touch start");
            if (_canInteract)
            {
                //var pointerPos = _inputs.Main.PointerPosition.ReadValue<Vector2>();
                //var posInPlane = ConvertScreenPositionToPlanePosition(pointerPos);

                //buildingWhenStartTouch = BuildingManager.Instance.HasBuildingAtPosition(posInPlane);
                //if (buildingWhenStartTouch != null)
                //{
                //    if (buildingWhenStartTouch == BuildingManager.Instance.SelectedBuilding)
                //    {
                //        BuildingManager.Instance.StartMove(posInPlane);
                //        _moveBuilding = true;
                //        return;
                //    }
                //}
                Debug.Log("Touch start");
                _moveMap = true;

            }
        }

        private void TouchCanceled()
        {
            Debug.Log("Touch cancel");
            _moveMap = false;
            _moveBuilding = false;

            if (buildingWhenStartTouch != null)
            {
                var pointerPos = _inputs.Main.PointerPosition.ReadValue<Vector2>();
                var posInPlane = ConvertScreenPositionToPlanePosition(pointerPos);

                var building = BuildingManager.Instance.HasBuildingAtPosition(posInPlane);
                if (building == buildingWhenStartTouch)
                {
                    BuildingManager.Instance.Select(building);
                }
            }

        }

        public Vector3 ConvertScreenPositionToPlanePosition(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Plane mapPlane = new Plane(Vector3.up, Vector3.zero); // Mặt phẳng y = 0, pháp vector là (0, 1, 0)

            float distance;
            if (mapPlane.Raycast(ray, out distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                return new Vector3(worldPosition.x, 0, worldPosition.z);
            }
            return Vector3.zero;
        }

        public void ActiveInteract(bool value)
        {
            _canInteract = value;
        }
    }
}