namespace ChaosAge.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ChaosAge.Config;
    using UnityEngine;

    public class PlayerData
    {
        public int Gold;
        public int Elixir;
        public int Gem;

        public List<int> buildingIds;
        public List<UnitData> units;

        public PlayerData()
        {
            buildingIds = new List<int>();
        }

        #region File
        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            Debug.LogWarning(json);
            PlayerPrefs.SetString("PLAYER_DATA", json);
        }

        public void SaveToFile(string filePath)
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Saved to: " + filePath);
        }

        public static PlayerData LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        #endregion

        public void AddBuilding(BuildingData buildingData)
        {
            buildingIds.Add(buildingData.id);
            Save();
        }

        public static PlayerData Load()
        {
            if (PlayerPrefs.HasKey("PLAYER_DATA"))
            {
                Debug.Log("Load player from PlayerPref");
                string json = PlayerPrefs.GetString("PLAYER_DATA");

                return JsonUtility.FromJson<PlayerData>(json);
            }
            Debug.Log("Load new player");
            return new PlayerData();
        }

        public List<BuildingData> GetListBuildingData()
        {
            var listBuildingData = new List<BuildingData>();
            foreach (var buildingId in buildingIds)
            {
                var buildingData = BuildingData.Load(buildingId);
                listBuildingData.Add(buildingData);
            }
            return listBuildingData;
        }
    }
}
