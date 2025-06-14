namespace ChaosAge.data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DatSystem;
    using UnityEngine;

    public class PlayerData
    {
        public int level;
        public int Gold;
        public int Elixir;
        public int Gem;

        public List<int> buildingIds;
        public List<int> units;

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

        public class BuildingFile
        {
            public List<BuildingData> listBuilding;
        }

        public void SaveToFile(string filePath)
        {
            var buildingFile = new BuildingFile();
            buildingFile.listBuilding = GetListBuildingData();
            string json = JsonUtility.ToJson(buildingFile, true);
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
            var playerData = new PlayerData();
            if (PlayerPrefs.HasKey("PLAYER_DATA"))
            {
                Debug.Log("Load player from PlayerPref");
                string json = PlayerPrefs.GetString("PLAYER_DATA");
                playerData = JsonUtility.FromJson<PlayerData>(json);
            }
            return playerData;
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

        public void AddResource(EResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case EResourceType.Gold:
                    Gold = Mathf.Clamp(
                        Gold + amount,
                        0,
                        DataManager.Instance.GetMaxResource(EResourceType.Gold)
                    );
                    break;
                case EResourceType.Elixir:
                    Elixir = Mathf.Clamp(
                        Elixir + amount,
                        0,
                        DataManager.Instance.GetMaxResource(EResourceType.Elixir)
                    );
                    break;
            }
            Save();
        }

        public int GetResource(EResourceType resourceType)
        {
            switch (resourceType)
            {
                case EResourceType.Gold:
                    return Gold;
                case EResourceType.Elixir:
                    return Elixir;
            }
            return 0;
        }

        public void ReduceResource(EResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case EResourceType.Gold:
                    Gold -= amount;
                    break;
                case EResourceType.Elixir:
                    Elixir -= amount;
                    break;
            }
            Save();
        }

        public int GetUnitNum(EUnitType unitType)
        {
            if (units == null || units.Count <= (int)unitType)
            {
                return 0;
            }
            return units[(int)unitType];
        }

        public void AddUnit(EUnitType unitType, int amount)
        {
            for (int i = units.Count; i <= (int)unitType; i++)
            {
                units.Add(0);
            }
            units[(int)unitType] += amount;
            Save();
        }

        public void ReduceUnit(EUnitType unitType, int amount)
        {
            units[(int)unitType] = Mathf.Max(0, units[(int)unitType] - amount);
            Save();
        }

    }
}
