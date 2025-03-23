using System;
using ChaosAge.Config;

namespace ChaosAge.Data
{
    [Serializable]
    public class BuildingData
    {
        private static int COUNT_BUILDING_ID = 0;

        public string id = "";
        public EBuildingType type;
        public int level = 0;

        public int x = 0;
        public int y = 0;

        public BuildingData()
        {

        }

        public BuildingData(EBuildingType type, int x = 0, int y = 0, int level = 0)
        {
            this.id = COUNT_BUILDING_ID.ToString();
            this.type = type;
            this.level = 0;
            this.x = x;
            this.y = y;
            COUNT_BUILDING_ID += 1;
        }

    }

}
