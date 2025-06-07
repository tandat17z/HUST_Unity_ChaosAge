namespace ChaosAge.building
{
    using System;
    using DatSystem;
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Building))]
    public class Mining : MonoBehaviour
    {
        [SerializeField]
        private Button _claimButton;

        [SerializeField, ReadOnly]
        private EResourceType _resourceType;

        [SerializeField, ReadOnly]
        private int _capacity;

        [SerializeField, ReadOnly]
        private DateTime _startClaimTime;

        [SerializeField, ReadOnly]
        private float _productionRate;

        private Building _building;

        void Awake()
        {
            _building = GetComponent<Building>();
            _building.OnInitialized += Init;
            _building.OnStopRunning += OnStopRunning;
            _claimButton.onClick.AddListener(() =>
            {
                var amount = Claim();
                DataManager.Instance.PlayerData.AddResource(_resourceType, amount);

                _claimButton.gameObject.SetActive(false);
                DOVirtual.DelayedCall(
                    5f,
                    () =>
                    {
                        _claimButton.gameObject.SetActive(true);
                    }
                );
            });
        }

        private void OnStopRunning()
        {
            _claimButton.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            _building.OnInitialized -= Init;
            _building.OnStopRunning -= OnStopRunning;
        }

        private void Init()
        {
            var ticks = long.Parse(PlayerPrefs.GetString($"{_building.Id}_MiningStartClaimTime"));
            _startClaimTime = new DateTime(ticks);

            var miningSO = SOManager.Instance.GetSO<MiningConfigSO>(
                $"{_building.Type}_{_building.Level}"
            );

            _resourceType = miningSO.capacity.resourceType;
            _capacity = miningSO.capacity.quantity;
            _productionRate = miningSO.productionPerMinute / 60f;

            _claimButton.GetComponent<UIButtonClaim>().SetIcon(_resourceType);
        }

        private int Claim()
        {
            var deltaTime = DateTime.Now - _startClaimTime;
            float amount = (float)deltaTime.TotalSeconds * _productionRate;
            _startClaimTime = DateTime.Now;
            PlayerPrefs.SetString(
                $"{_building.Id}_MiningStartClaimTime",
                _startClaimTime.Ticks.ToString()
            );
            amount = Mathf.Min(amount, _capacity);
            return (int)amount;
        }
    }
}
