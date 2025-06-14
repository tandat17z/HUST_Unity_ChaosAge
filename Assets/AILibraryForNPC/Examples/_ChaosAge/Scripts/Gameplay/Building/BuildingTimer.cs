namespace ChaosAge.building
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [RequireComponent(typeof(Building))]
    public class BuildingTimer : MonoBehaviour
    {
        private Building _building;
        private BuildingVisual _buildingVisual;

        private bool _isTimerRunning = false;

        [SerializeField, ReadOnly]
        private float _duration;

        [SerializeField, ReadOnly]
        private float _remainingTime;

        public Action OnTimerEnded { get; set; }
        public DateTime _startTime;
        public bool IsTimerRunning
        {
            get => _isTimerRunning;
        }

        private void Awake()
        {
            _building = GetComponent<Building>();
            _buildingVisual = GetComponent<BuildingVisual>();

            _isTimerRunning = false;
            _remainingTime = 0;

            OnTimerEnded += OnComplete;
            _building.OnStopRunning += StopTimer;
            _building.OnInitialized += Init;
        }

        private void Init()
        {
            try
            {
                Debug.Log($"Init {_building.Id}_StartUpgradeTime");
                var ticks = long.Parse(PlayerPrefs.GetString($"{_building.Id}_StartUpgradeTime"));
                _startTime = new DateTime(ticks);
            }
            catch (Exception e)
            {
                _startTime = DateTime.Now;
                return;
            }

            var buildingConfigSO = SOManager.Instance.GetSO<BuildingConfigSO>(
                $"{_building.Type}_{_building.Level + 1}"
            );
            if (buildingConfigSO == null)
            {
                _duration = 3600;
            }
            else
            {
                _duration = buildingConfigSO.timeToBuild;
            }

            var deltaTime = DateTime.Now - _startTime;
            _remainingTime = _duration - (float)deltaTime.TotalSeconds;
            if (_remainingTime <= 0)
            {
                _isTimerRunning = false;
                OnTimerEnded?.Invoke();
            }
            else
            {
                _isTimerRunning = true;
                _buildingVisual = GetComponent<BuildingVisual>();
                _buildingVisual.ShowUpgradeUI();
            }
        }

        private void OnDestroy()
        {
            OnTimerEnded -= OnComplete;
            _building.OnStopRunning -= StopTimer;
            _building.OnInitialized -= Init;
        }

        private void OnComplete()
        {
            StopTimer();
            _remainingTime = 0;
            PlayerPrefs.DeleteKey($"{_building.Id}_StartUpgradeTime");
        }

        public void StartTimer(float duration)
        {
            _startTime = DateTime.Now;
            PlayerPrefs.SetString($"{_building.Id}_StartUpgradeTime", _startTime.Ticks.ToString());

            _duration = duration;
            _remainingTime = duration;
            _isTimerRunning = true;
        }

        public void StopTimer()
        {
            _isTimerRunning = false;
        }

        public void CompleteUpgradeByTime()
        {
            _remainingTime = 0.1f;
        }

        void Update()
        {
            if (_isTimerRunning)
            {
                _remainingTime -= Time.deltaTime;
                _buildingVisual.SetTime(_remainingTime, _duration);

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
