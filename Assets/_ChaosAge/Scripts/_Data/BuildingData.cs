using System;
using ChaosAge.Config;
using UnityEngine;

namespace ChaosAge.Data
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

        public bool complete;
        public float buildTime;

        public BuildingData(EBuildingType type, int x, int y)
        {
            // this.id = id;
            this.type = type;
            this.x = x;
            this.y = y;

            this.level = 1;
            this.complete = true;
        }

        public BuildingData() { }
    }
}
