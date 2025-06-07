using System;
using ChaosAge.data;
using UnityEngine;

namespace ChaosAge.data
{
    [Serializable]
    public class BuildingData
    {
        //     public static string BuildingIdKey = "BUILDING_ID";
        //     public static int CountBuildingId
        //     {
        //         get => PlayerPrefs.GetInt(BuildingIdKey, 0);
        //         set => PlayerPrefs.SetInt(BuildingIdKey, value);
        //     }

        public int id;
        public EBuildingType type;
        public int level;
        public int x;
        public int y;

        public float remainingTime;

        public BuildingData(
            int id,
            EBuildingType type,
            int level,
            Vector2 gridPosition,
            float remainingTime = 0
        )
        {
            this.id = id;
            this.type = type;
            this.level = level;
            this.x = (int)gridPosition.x;
            this.y = (int)gridPosition.y;
            this.remainingTime = remainingTime;
        }

        public static BuildingData Load(int buildingId)
        {
            var json = PlayerPrefs.GetString($"MY_BUILDING_{buildingId}");
            Debug.Log($"Load buiding {buildingId} : {json}");
            return JsonUtility.FromJson<BuildingData>(json);
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(this);
            Debug.Log($"Save building {id} : {json}");
            PlayerPrefs.SetString($"MY_BUILDING_{id}", json);
        }
    }

    [Serializable]
    public enum EBuildingType
    {
        TownHall,
        GoldMine,
        GoldStorage,
        ElixirMine,
        ElixirStorage,
        BuildersHut,
        Cannon,
        ArcherTowner,
        Wall,
    }
}
