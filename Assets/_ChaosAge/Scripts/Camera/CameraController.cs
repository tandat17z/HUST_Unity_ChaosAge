using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChaosAge.input;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;

namespace ChaosAge.camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private float moveSpeed = 50;
        [SerializeField] private float moveSmooth = 5;

        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float zoomSmooth = 5f;

        [Header("")]
        [SerializeField] private float right = 50;
        [SerializeField] private float left = 50;
        [SerializeField] private float up = 50;
        [SerializeField] private float down = 50;
        [SerializeField] private float angle = 45;
        [SerializeField] private float zoom = 5;
        [SerializeField] private float zoomMax = 10;
        [SerializeField] private float zoomMin = 1;

        private Controls _inputs = null;

        private Transform _root;
        private Transform _pivot;
        private Transform _target;


        // bounds
        private Vector3 _center; // tâm của plane

        // state
        public bool CanInteract { get => _canInteract; set => _canInteract = value; }
        private bool _canInteract;
        private bool _moving;
        private bool _zooming;

        // 
        private Vector2 _zoomPositionOnScreen = Vector2.zero;
        private Vector3 _zoomPositionOnWorld = Vector3.zero;
        private float _zoomBaseValue = 0;
        private float _zoomBaseDistance = 0;

        private bool _movingBuilding;
        private Vector3 _buildingBasePosition;




        #region Init
        private void Awake()
        {
            _inputs = new Controls();
            _root = new GameObject("CameraHelper").transform; // vị trị cam chiếu tới plane
            _pivot = new GameObject("CameraPivot").transform; // để quay cam
            _target = new GameObject("CameraTarget").transform; // vị trí của cam

            _pivot.SetParent(_root);
            _target.SetParent(_pivot);

            camera.orthographic = true;
            camera.nearClipPlane = 0;
        }

        private void Start()
        {
            //Initialize(Vector3 center, float right, float left, float up, float down, float angle, float zoom, float zoomMin, float zoomMax)
            //Initialize(Vector3.zero, 40, 40, 40, 40, 45, 10, 5, 20);
            Initialize(Vector3.zero);
        }

        private void Initialize(Vector3 center)
        {
            _center = Vector3.zero;

            _canInteract = true;
            _moving = false;
            _zooming = false;

            _pivot.SetParent(_root);
            _target.SetParent(_pivot);

            _root.position = center;
            _root.localEulerAngles = Vector3.zero;

            _pivot.localPosition = Vector3.zero;
            _pivot.localEulerAngles = new Vector3(this.angle, 0, 0);

            _target.localPosition = new Vector3(0, 0, -100);
            _target.localEulerAngles = Vector3.zero;

            camera.transform.rotation = _target.rotation;
            camera.orthographicSize = this.zoom;

            //
            _movingBuilding = false;
        }

        private void OnEnable()
        {
            _inputs.Enable();

            _inputs.Main.Move.started += _ => MoveStarted();
            _inputs.Main.Move.canceled += _ => MoveCanceled();


            _inputs.Main.TouchZoom.started += _ => ZoomStarted();
            _inputs.Main.TouchZoom.canceled += _ => ZoomCanceled();
        }

        private void OnDisable()
        {
            _inputs.Disable();

            _inputs.Main.Move.started -= _ => MoveStarted();
            _inputs.Main.Move.canceled -= _ => MoveCanceled();


            _inputs.Main.TouchZoom.started -= _ => ZoomStarted();
            _inputs.Main.TouchZoom.canceled -= _ => ZoomCanceled();
        }
        #endregion

        #region Convert
        public Vector3 CameraScreenPositionToPlanePosition(Vector2 screenPosition)
        {
            Ray ray = camera.ScreenPointToRay(screenPosition);
            Plane mapPlane = new Plane(Vector3.up, Vector3.zero); // Mặt phẳng y = 0, pháp vector là (0, 1, 0)

            float distance;
            if (mapPlane.Raycast(ray, out distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                return new Vector3(worldPosition.x, 0, worldPosition.z);
            }
            return Vector3.zero;
        }
        #endregion

        #region Start, end move/ zoom
        private void MoveStarted()
        {
            if (_canInteract)
            {
                // Nếu đã chọn building thì tương tác với công trình
                var pointerPos = _inputs.Main.PointerPosition.ReadValue<Vector2>();
                var poinerPosInPlane = CameraScreenPositionToPlanePosition(pointerPos);
                Debug.Log($"Pointer {poinerPosInPlane}");

                var selectedBuidling = BuildingManager.Instance.SelectedBuilding;
                if (selectedBuidling != null)
                {
                    _buildingBasePosition = poinerPosInPlane;

                    if (BuildingManager.Instance.Grid.IsWorldPositionIsOnPlane(_buildingBasePosition, selectedBuidling))
                    {
                        selectedBuidling.StartMovingOnGrid();
                        _movingBuilding = true;
                    }
                }
                else
                {
                    BuildingManager.Instance.SelectBuilding(poinerPosInPlane);
                }

                if (_movingBuilding == false)
                {
                    _moving = true;

                }
            }
        }

        private void MoveCanceled()
        {
            _moving = false;
            _movingBuilding = false;
        }

        private void ZoomStarted()
        {
            if (_canInteract)
            {
                Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<Vector2>();
                Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<Vector2>();
                _zoomPositionOnScreen = Vector2.Lerp(touch0, touch1, 0.5f);
                _zoomPositionOnWorld = CameraScreenPositionToPlanePosition(_zoomPositionOnScreen);
                _zoomBaseValue = zoom;

                touch0.x /= Screen.width;
                touch1.x /= Screen.width;

                touch0.y /= Screen.height;
                touch1.y /= Screen.height;

                _zoomBaseDistance = Vector2.Distance(touch0, touch1);

                _zooming = true;
            }
        }

        private void ZoomCanceled()
        {
            _zooming = false;
        }
        #endregion

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.touchSupported == false)
            {
                float mouseScroll = _inputs.Main.MouseScroll.ReadValue<float>();
                if (mouseScroll > 0)
                {
                    zoom -= 10f * Time.deltaTime;
                }
                else if (mouseScroll < 0)
                {
                    zoom += 10f * Time.deltaTime;
                }
            }
#endif
            if (_moving)
            {
                Vector2 move = _inputs.Main.MoveDelta.ReadValue<Vector2>();
                if (move != Vector2.zero)
                {
                    move.x /= Screen.width;
                    move.y /= Screen.height;

                    _root.position -= _root.right.normalized * move.x * moveSpeed;
                    _root.position -= _root.forward.normalized * move.y * moveSpeed;
                }
            }
            else if (_zooming)
            {
                Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<Vector2>();
                Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<Vector2>();

                touch0.x /= Screen.width;
                touch1.x /= Screen.width;

                touch0.y /= Screen.height;
                touch1.y /= Screen.height;

                float currentDistance = Vector2.Distance(touch0, touch1);
                float deltaDistance = currentDistance - _zoomBaseDistance;
                zoom = _zoomBaseValue - (deltaDistance * zoomSpeed);

                Vector3 zoomCenter = CameraScreenPositionToPlanePosition(_zoomPositionOnScreen);
                _root.position += (_zoomPositionOnWorld - zoomCenter);
            }

            AdjustBounds();

            if (camera.orthographicSize != zoom)
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoom, zoomSmooth * Time.deltaTime);

            }
            if (camera.transform.position != _target.position)
            {
                camera.transform.position = Vector3.Lerp(camera.transform.position, _target.position, moveSmooth * Time.deltaTime);
            }

            // Di chuyển công trình
            if (_movingBuilding)
            {
                var pointerPos = _inputs.Main.PointerPosition.ReadValue<Vector2>();
                var currentPosition = CameraScreenPositionToPlanePosition(pointerPos);

                BuildingManager.Instance.SelectedBuilding.UpdateGridPosition(_buildingBasePosition, currentPosition);
            }
        }

        #region Bounds
        private void AdjustBounds()
        {
            if (zoom < zoomMin)
            {
                zoom = zoomMin;
            }
            if (zoom > zoomMax)
            {
                zoom = zoomMax;
            }

            float h = PlaneOrthographicSize();
            float w = camera.aspect * h;

            if (h > (up + down) / 2f)
            {
                float n = (up + down) / 2f;
                zoom = n * Mathf.Sin(angle * Mathf.Deg2Rad);
            }

            if (w > (right + left) / 2f)
            {
                float n = (right + left) / 2f;
                zoom = n * Mathf.Sin(angle * Mathf.Deg2Rad) / camera.aspect;
            }

            Vector3 tr = _root.position + _root.right.normalized * w + _root.forward.normalized * h;
            Vector3 tl = _root.position - _root.right.normalized * w + _root.forward.normalized * h;
            Vector3 dr = _root.position + _root.right.normalized * w - _root.forward.normalized * h;
            Vector3 dl = _root.position - _root.right.normalized * w - _root.forward.normalized * h;

            if (tr.x > _center.x + right)
            {
                _root.position += Vector3.left * Mathf.Abs(tr.x - (_center.x + right));
            }
            if (tl.x < _center.x - left)
            {
                _root.position += Vector3.right * Mathf.Abs(-tl.x + (_center.x - left));
            }

            if (tr.z > _center.z + up)
            {
                _root.position += Vector3.back * Mathf.Abs(tr.z - (_center.z + up));
            }

            if (dl.z < _center.z - down)
            {
                _root.position += Vector3.forward * Mathf.Abs(-dl.z + (_center.z - down));
            }

        }

        private float PlaneOrthographicSize()
        {
            float h = zoom * 2f;
            return h / Mathf.Sin(angle * Mathf.Deg2Rad) / 2;
        }
        #endregion
    }

}