using System;
using System.Collections.Generic;
using ChaosAge.Data;

namespace ChaosAge.Config
{
    [Serializable]
    public class BuildingConfig
    {
        public string type;
        public int maxNumber;

        public int columns = 0;
        public int rows = 0;

        public List<LevelBuildingConfig> levelBuildingConfig;
        public List<LevelBattleBuildingConfig> levelBattleBuildingConfig;

        public BuildingConfig(EBuildingType type)
        {
            this.type = type.ToString();
            maxNumber = 1;

            columns = 4;
            rows = 4;

            levelBuildingConfig = new List<LevelBuildingConfig>();
            levelBattleBuildingConfig = new List<LevelBattleBuildingConfig>();
            for (int i = 1; i <= 5; i++)
            {
                levelBuildingConfig.Add(new LevelBuildingConfig(i));
                levelBattleBuildingConfig.Add(new LevelBattleBuildingConfig(i));
            }
        }
    }

    [Serializable]
    public class LevelBuildingConfig
    {
        public int level;

        public int requireGold;
        public int requireElixir;
        public int requireGem;

        public float timeBuild;

        public LevelBuildingConfig(int level)
        {
            this.level = level;
            requireGold = level * 10;
            requireElixir = 0;
            requireGem = 0;

            timeBuild = 30 * level;
        }
    }

    [Serializable]
    public class LevelBattleBuildingConfig
    {
        public int level;

        public int storage = 0;
        public DateTime boost;
        public int health = 100;
        public float damage = 0;
        public int capacity = 0;
        public float speed = 0;
        public float radius = 0;
        public DateTime constructionTime;
        public bool isConstructing = false;
        public int buildTime = 0;
        public BuildingTargetType targetType = BuildingTargetType.none;
        public float blindRange = 0; // Vùng mù
        public float splashRange = 0;
        public float rangedSpeed = 5;
        public float percentage = 0;

        public LevelBattleBuildingConfig(int level)
        {
            this.level = level;
        }
    }

    [Serializable]
    public enum EBuildingType
    {
        townhall,
        goldmine,
        goldstorage,
        elixirmine,
        elixirstorage,
        buildershut,
        armycamp,
        barracks,
        wall,
        cannon,
        archertower,
        //mortor, airdefense, wizardtower, hiddentesla, bombtower, xbow, infernotower, decoration, obstacle, boomb, springtrap, airbomb, giantbomb, seekingairmine, skeletontrap
    }
}

