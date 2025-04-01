using System;
using ChaosAge.Config;
using UnityEngine;

namespace ChaosAge.Data
{
    [Serializable]
    public class BuildingData
    {
        public static string BuildingIdKey = "BUILDING_ID";
        public static int CountBuildingId { get => PlayerPrefs.GetInt(BuildingIdKey, 0); set => PlayerPrefs.SetInt(BuildingIdKey, value); }

        public int id;
        public EBuildingType type;
        public int level;
        public int x;
        public int y;

        public DateTime buildTime;
        public bool complete;

        public BuildingData(EBuildingType type, int v1, int v2)
        {
            id = CountBuildingId;
            this.type = type;
            this.x = v1;
            this.y = v2;

            this.level = 1;
            this.complete = true;
            CountBuildingId = CountBuildingId + 1;
        }

        public BuildingData()
        {

        }
    }
}

