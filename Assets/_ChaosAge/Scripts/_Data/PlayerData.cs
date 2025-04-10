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

        public List<BuildingData> buildings;
        public List<UnitData> units;

        public PlayerData()
        {
            buildings = new List<BuildingData>();
            buildings.Add(new BuildingData(EBuildingType.townhall, 20, 20));
            buildings.Add(new BuildingData(EBuildingType.buildershut, 25, 20));
            buildings.Add(new BuildingData(EBuildingType.goldmine, 28, 20));
            buildings.Add(new BuildingData(EBuildingType.armycamp, 32, 20));
        }

        #region File
        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            Debug.Log(json);
            PlayerPrefs.SetString("PLAYER_DATA", json);
            PlayerPrefs.Save();
        }

        public void SaveToFile(string filePath)
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Saved to: " + filePath);
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

        public static PlayerData LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        #endregion

        public void AddBuiling(BuildingData buildingData)
        {
            buildings.Add(buildingData);
            Save();
        }

        public void UpdateBuildingData(BuildingData buildingData)
        {
            int index = buildings.FindIndex(b => b.id == buildingData.id);
            if (index != -1)
            {
                buildings[index] = buildingData;
                Save();
            }
        }

        public int GetBuildingNumber(EBuildingType type)
        {
            int num = 0;
            foreach (var building in buildings)
            {
                if (building.type == type)
                {
                    num += 1;
                }
            }
            return num;
        }
    }

}
