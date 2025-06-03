using System;
using ChaosAge;
using ChaosAge.Config;
using ChaosAge.Data;
using DatSystem.utils;
using UnityEngine;

namespace DatSystem
{
    public class DataManager : Singleton<DataManager>
    {
        protected override void OnAwake() { }

        private int CountBuildingId
        {
            get => PlayerPrefs.GetInt("COUNT_BUILDING_ID", 0);
            set => PlayerPrefs.SetInt("COUNT_BUILDING_ID", value);
        }
        private PlayerData _playerData;
        private GameConfig _gameConfig;
        public PlayerData PlayerData
        {
            get { return _playerData; }
        }
        public GameConfig GameConfig
        {
            get { return _gameConfig; }
        }

        public void LoadPlayerData()
        {
            _playerData = PlayerData.Load();
            if (_playerData.buildingIds.Count == 0) // new player
            {
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.TownHall, new Vector2(20, 20))
                );
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.BuildersHut, new Vector2(25, 20))
                );
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.GoldMine, new Vector2(28, 20))
                );
                _playerData.AddBuilding(
                    CreateBuilding(EBuildingType.GoldStorage, new Vector2(32, 20))
                );
            }
        }

        public BuildingData CreateBuilding(EBuildingType type, Vector2 position, bool isSave = true)
        {
            var buildingData = new BuildingData(CountBuildingId, type, 0, position);
            CountBuildingId++;
            if (isSave)
            {
                buildingData.Save();
            }
            return buildingData;
        }

        public void LoadGameConfig()
        {
            _gameConfig = GameConfig.LoadFromFile("Assets/_ChaosAge/Config.json");
        }

        internal void SaveBuilding(Building building)
        {
            var buildingData = building.GetData();
            buildingData.Save();
        }
    }
}
