using System;
using System.Collections.Generic;

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

        public BuildingConfig(EBuildingType type)
        {
            this.type = type.ToString();
            maxNumber = 1;

            columns = 4;
            rows = 4;

            levelBuildingConfig = new List<LevelBuildingConfig>();
            for (int i = 1; i <= 5; i++)
            {
                levelBuildingConfig.Add(new LevelBuildingConfig(i));
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
    public enum EBuildingType
    {
        townhall,
        goldmine,
        goldstorage,
        elixirmine,
        elixirstorage,
        darkelixirmine,
        darkelixirstorage,
        buildershut,
        armycamp,
        barracks,
        wall,
        cannon,
        archertower,
        //mortor, airdefense, wizardtower, hiddentesla, bombtower, xbow, infernotower, decoration, obstacle, boomb, springtrap, airbomb, giantbomb, seekingairmine, skeletontrap
    }
}

