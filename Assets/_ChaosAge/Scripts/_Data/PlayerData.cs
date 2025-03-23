namespace ChaosAge.Data
{
    using System.Collections.Generic;
    using ChaosAge.Config;
    using UnityEngine;

    public class PlayerData
    {
        public int Gold;
        public int Elixir;
        public int Gem;

        public List<BuildingData> buildings;

        public PlayerData()
        {
            buildings = new List<BuildingData>();
            buildings.Add(new BuildingData(EBuildingType.TownHall, 20, 20));
            buildings.Add(new BuildingData(EBuildingType.BuilderHut, 25, 20));
            buildings.Add(new BuildingData(EBuildingType.GoldMine, 28, 20));
            buildings.Add(new BuildingData(EBuildingType.ArmyCamp, 32, 20));
        }
        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            Debug.Log(json);
            PlayerPrefs.SetString("PLAYER_DATA", json);
            PlayerPrefs.Save();
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

        public void AddBuiling(BuildingData buildingData)
        {
            buildings.Add(buildingData);
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

    }

}
