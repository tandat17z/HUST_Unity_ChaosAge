using System.Collections.Generic;

namespace ChaosAge.Config
{
    public class GameConfig
    {
        List<BuildingConfig> buildings;

        public BuildingConfig GetBuildingConfig(EBuildingType type)
        {
            return new BuildingConfig();
        }
    }

}
