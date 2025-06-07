namespace ChaosAge.building
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [RequireComponent(typeof(Building))]
    public class BuildingTimer : MonoBehaviour
    {
        private Building building;
        private BuildingVisual buildingVisual;

        private bool _isTimerRunning = false;

        [SerializeField, ReadOnly]
        private float _duration;

        [SerializeField, ReadOnly]
        private float _remainingTime;

        public Action OnTimerEnded { get; set; }
        public bool IsTimerRunning
        {
            get => _isTimerRunning;
        }

        private void Start()
        {
            building = GetComponent<Building>();
            buildingVisual = building.BuildingVisual;

            _isTimerRunning = false;
            _remainingTime = 0;

            OnTimerEnded += OnComplete;
        }

        private void OnComplete()
        {
            StopTimer();
            _remainingTime = 0;
        }

        public void StartTimer(float duration)
        {
            _duration = duration;
            _remainingTime = duration;
            _isTimerRunning = true;
        }

        public void StopTimer()
        {
            _isTimerRunning = false;
        }

        void Update()
        {
            if (_isTimerRunning)
            {
                _remainingTime -= Time.deltaTime;
                buildingVisual.SetTime(_remainingTime, _duration);

                if (_remainingTime <= 0)
                {
                    OnTimerEnded?.Invoke();
                }
            }
        }

        public float GetRemainingTime()
        {
            return _remainingTime;
        }
    }
}
