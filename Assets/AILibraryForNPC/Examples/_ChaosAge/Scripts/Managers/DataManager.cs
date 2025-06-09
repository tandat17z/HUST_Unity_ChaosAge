using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.data;
using ChaosAge.manager;
using DatSystem.utils;
using UnityEngine;

namespace DatSystem
{
    public class DataManager : Singleton<DataManager>
    {
        protected override void OnAwake()
        {
            BuildingManager.OnCompleteUpgrade += UpdateMaxResource;
        }

        private int CountBuildingId
        {
            get => PlayerPrefs.GetInt("COUNT_BUILDING_ID", 0);
            set => PlayerPrefs.SetInt("COUNT_BUILDING_ID", value);
        }
        private PlayerData _playerData;
        public PlayerData PlayerData
        {
            get { return _playerData; }
        }
        private Dictionary<EResourceType, int> _dictMaxResource = new();

        public void LoadPlayerData()
        {
            _playerData = PlayerData.Load();
            if (_playerData.buildingIds.Count == 0) // new player
            {
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.TownHall, 1, new Vector2(20, 20))
                );
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.BuildersHut, 1, new Vector2(25, 20))
                );
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.GoldMine, 1, new Vector2(28, 20))
                );
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.GoldStorage, 1, new Vector2(32, 20))
                );
            }
        }

        public BuildingData CreateBuilding(
            EBuildingType type,
            int level,
            Vector2 position,
            bool isSave = true
        )
        {
            var buildingData = new BuildingData(CountBuildingId, type, level, position);
            CountBuildingId++;
            if (isSave)
            {
                buildingData.Save();
            }
            return buildingData;
        }

        public void SaveBuilding(Building building)
        {
            var buildingData = building.GetData();
            buildingData.Save();
        }

        public int GetMaxResource(EResourceType resourceType)
        {
            return _dictMaxResource[resourceType];
        }

        public void UpdateMaxResource()
        {
            _dictMaxResource.Clear();
            _dictMaxResource[EResourceType.Gold] = 0;
            _dictMaxResource[EResourceType.Elixir] = 0;
            foreach (var building in BuildingManager.Instance.Buildings)
            {
                var buildingConfig = building.BuildingConfigSO;
                switch (building.Type)
                {
                    case EBuildingType.TownHall:
                        var townHallSO = buildingConfig as TownhallConfigSO;
                        foreach (var capa in townHallSO.capacities)
                        {
                            _dictMaxResource[capa.resourceType] += capa.quantity;
                        }
                        break;
                    case EBuildingType.GoldStorage:
                    case EBuildingType.ElixirStorage:
                        var storageSO = buildingConfig as StorageConfigSO;
                        if (storageSO != null)
                        {
                            var capacity = storageSO.capacity;
                            _dictMaxResource[capacity.resourceType] += capacity.quantity;
                        }
                        break;
                }
            }
        }
    }
}
