using System;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.input
{
    public class TouchControllerInCity : Singleton<TouchControllerInCity>
    {
        [SerializeField]
        private float _tapThreshold = 0.2f;

        [SerializeField]
        private float _tapDistance = 30f;

        private Vector2 touchStartPos;
        private float touchStartTime;

        public TouchState TouchState { get; set; }
        public Action<Vector3> OnTouchStarted;
        public Action<Vector3> OnTouchMoved;
        public Action<Vector3> OnTouchEnded;
        public Action<Vector3> OnTap;

        public Action<float> OnTouchZoom;

        protected override void OnAwake() { }

        void Update()
        {
            HandleTouch();
        }

        void HandleTouch()
        {
            int count = Input.touchCount;

            if (count == 1)
            {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    touchStartPos = t.position;
                    touchStartTime = Time.time;
                    OnTouchStarted?.Invoke(GetTouchPositionInWorld(t.position));
                }
                else if (t.phase == TouchPhase.Moved)
                {
                    Vector3 delta =
                        GetTouchPositionInWorld(t.position)
                        - GetTouchPositionInWorld(t.deltaPosition);
                    OnTouchMoved?.Invoke(delta);
                }
                else if (t.phase == TouchPhase.Ended)
                {
                    float duration = Time.time - touchStartTime;
                    float distance = Vector2.Distance(t.position, touchStartPos);
                    OnTouchEnded?.Invoke(GetTouchPositionInWorld(t.position));

                    if (duration < _tapThreshold && distance < _tapDistance)
                    {
                        OnTap?.Invoke(GetTouchPositionInWorld(t.position));
                    }
                }
            }
            else if (count == 2)
            {
                Touch t0 = Input.GetTouch(0);
                Touch t1 = Input.GetTouch(1);

                Vector2 prev0 = t0.position - t0.deltaPosition;
                Vector2 prev1 = t1.position - t1.deltaPosition;

                float prevDist = Vector2.Distance(prev0, prev1);
                float currDist = Vector2.Distance(t0.position, t1.position);

                float delta = prevDist - currDist;

                OnTouchZoom?.Invoke(delta);
            }
        }

        public Vector3 GetTouchPositionInWorld(Vector2 touchPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // mặt phẳng y = 0

            float enter;
            if (groundPlane.Raycast(ray, out enter))
            {
                Vector3 worldPos = ray.GetPoint(enter);
                return worldPos;
            }
            return Vector3.zero;
        }
    }

    public enum TouchState
    {
        MovingMap,
        MovingBuilding,
        Zooming,
    }
}
