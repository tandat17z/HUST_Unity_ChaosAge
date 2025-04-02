using System;
using System.Collections.Generic;
using System.IO;
using ChaosAge.Data;
using UnityEngine;

namespace ChaosAge.Config
{
    [Serializable]
    public class GameConfig
    {
        public List<BuildingConfig> buildingConfigs;

        public GameConfig()
        {
            buildingConfigs = new();

            buildingConfigs.Add(new BuildingConfig(EBuildingType.townhall));
            buildingConfigs.Add(new BuildingConfig(EBuildingType.buildershut));
            buildingConfigs.Add(new BuildingConfig(EBuildingType.goldmine));
            buildingConfigs.Add(new BuildingConfig(EBuildingType.goldstorage));
            buildingConfigs.Add(new BuildingConfig(EBuildingType.armycamp));
            buildingConfigs.Add(new BuildingConfig(EBuildingType.cannon));
            buildingConfigs.Add(new BuildingConfig(EBuildingType.archertower));
            buildingConfigs.Add(new BuildingConfig(EBuildingType.wall));

            SaveToFile("config.json");
        }

        public void SaveToFile(string filePath)
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Saved to: " + filePath);
        }

        public static GameConfig LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameConfig>(json);
        }


        //
        public (int, int) GetBuildingSize(EBuildingType type)
        {
            foreach (var buildingConfig in buildingConfigs)
            {
                if (buildingConfig.type == type.ToString())
                {
                    return (buildingConfig.rows, buildingConfig.columns);
                }
            }
            return (0, 0);
        }

        public int GetBuildingMaxNumber(EBuildingType type)
        {
            foreach (var buildingConfig in buildingConfigs)
            {
                if (buildingConfig.type == type.ToString())
                {
                    return buildingConfig.maxNumber;
                }
            }
            return 0;
        }

        public BattleBuildingData GetBattleBuildingData(EBuildingType type, int level)
        {
            var data = new BattleBuildingData();
            data.type = type;
            data.level = level;
            foreach (var buildingConfig in buildingConfigs)
            {
                if (buildingConfig.type == type.ToString())
                {
                    data.SetInfo(buildingConfig);
                    break;
                }
            }
            return data;
        }
    }

}
