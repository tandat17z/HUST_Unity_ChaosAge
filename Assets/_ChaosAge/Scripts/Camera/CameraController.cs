using UnityEngine;

namespace ChaosAge.camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private float moveSpeed = 50;

        [SerializeField]
        private float moveSmooth = 5;

        [SerializeField]
        private float zoomSmooth = 5f;

        [SerializeField]
        private float right = 50;

        [SerializeField]
        private float left = 50;

        [SerializeField]
        private float up = 50;

        [SerializeField]
        private float down = 50;

        [SerializeField]
        private float angle = 45;

        [SerializeField]
        private float zoom = 5;

        [SerializeField]
        private float zoomMax = 10;

        [SerializeField]
        private float zoomMin = 1;

        private Transform _root;
        private Transform _pivot;
        private Transform _target;
        private Vector3 _center;

        void Awake()
        {
            _root = new GameObject("CameraHelper").transform;
            _pivot = new GameObject("CameraPivot").transform;
            _target = new GameObject("CameraTarget").transform;

            _pivot.SetParent(_root);
            _target.SetParent(_pivot);

            camera.orthographic = true;
            camera.nearClipPlane = 0;
        }

        void Start()
        {
            Initialize(Vector3.zero);
        }

        private void Initialize(Vector3 center)
        {
            _center = center;

            _pivot.SetParent(_root);
            _target.SetParent(_pivot);

            _root.position = center;
            _root.localEulerAngles = Vector3.zero;

            _pivot.localPosition = Vector3.zero;
            _pivot.localEulerAngles = new Vector3(angle, 0, 0);

            _target.localPosition = new Vector3(0, 0, -100);
            _target.localEulerAngles = Vector3.zero;

            camera.transform.rotation = _target.rotation;
            camera.orthographicSize = zoom;
        }

        void Update()
        {
            // HandleInput();
            AdjustBounds();

            camera.orthographicSize = Mathf.Lerp(
                camera.orthographicSize,
                zoom,
                zoomSmooth * Time.deltaTime
            );
            camera.transform.position = Vector3.Lerp(
                camera.transform.position,
                _target.position,
                moveSmooth * Time.deltaTime
            );
        }

        // private void HandleInput()
        // {
        //     float h = Input.GetAxis("Horizontal");
        //     float v = Input.GetAxis("Vertical");

        //     Vector2 moveDelta = new Vector2(h, v);
        //     Move(moveDelta);

        //     float scroll = Input.GetAxis("Mouse ScrollWheel");
        //     Zoom(-scroll * 5f);
        // }

        public void Move(Vector2 delta)
        {
            _root.position -= _root.right.normalized * delta.x * moveSpeed * Time.deltaTime;
            _root.position -= _root.forward.normalized * delta.y * moveSpeed * Time.deltaTime;
        }

        public void Zoom(float deltaZoom)
        {
            zoom = Mathf.Clamp(zoom + deltaZoom, zoomMin, zoomMax);
        }

        private void AdjustBounds()
        {
            zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

            float h = zoom * 2f / Mathf.Sin(angle * Mathf.Deg2Rad) / 2;
            float w = camera.aspect * h;

            Vector3 tr = _root.position + _root.right.normalized * w + _root.forward.normalized * h;
            Vector3 tl = _root.position - _root.right.normalized * w + _root.forward.normalized * h;
            Vector3 dl = _root.position - _root.right.normalized * w - _root.forward.normalized * h;

            if (tr.x > _center.x + right)
                _root.position += Vector3.left * Mathf.Abs(tr.x - (_center.x + right));
            if (tl.x < _center.x - left)
                _root.position += Vector3.right * Mathf.Abs(-tl.x + (_center.x - left));

            if (tr.z > _center.z + up)
                _root.position += Vector3.back * Mathf.Abs(tr.z - (_center.z + up));
            if (dl.z < _center.z - down)
                _root.position += Vector3.forward * Mathf.Abs(-dl.z + (_center.z - down));
        }
    }
}
