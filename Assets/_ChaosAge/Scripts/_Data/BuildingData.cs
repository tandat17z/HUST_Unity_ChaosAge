using ChaosAge.Config;

namespace ChaosAge.Data
{
    public class BuildingData
    {
        private static int COUNT_BUILDING_ID = 0;

        public string id = "";
        public EBuildingType type;
        public int level = 0;

        public int x = 0;
        public int y = 0;
        public int columns = 0;
        public int rows = 0;

        public BuildingData()
        {

        }

        public BuildingData(EBuildingType type)
        {
            this.id = COUNT_BUILDING_ID.ToString();
            this.type = type;
            this.level = 0;

            COUNT_BUILDING_ID += 1;
        }
    }

}
