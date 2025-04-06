using System;
using ChaosAge.Config;

namespace ChaosAge.Data
{
    [Serializable]
    public class BattleBuildingData
    {
        public static int COUNT_BUILDING_ID = 0;

        public int id = 0;
        public EBuildingType type;
        public int level = 0;

        public int x = 0;
        public int y = 0;
        public int columns = 0;
        public int rows = 0;
        public int storage = 0;
        public DateTime boost;
        public int health = 100;
        public float damage = 0;
        public int capacity = 0;
        public float speed = 0; // tốc độ bắn
        public float radius = 0; // bán kính
        public DateTime constructionTime;
        public bool isConstructing = false;
        public int buildTime = 0;
        public BuildingTargetType targetType = BuildingTargetType.none; // loại type
        public float blindRange = 0; // Vùng mù
        public float splashRange = 0;
        public float rangedSpeed = 5; // tốc độ bay của projectile
        public float percentage = 0;

        public BattleBuildingData()
        {

        }

        public BattleBuildingData(EBuildingType type, int x = 0, int y = 0, int level = 0)
        {
            this.id = COUNT_BUILDING_ID;
            this.type = type;
            this.level = 0;
            this.x = x;
            this.y = y;
            COUNT_BUILDING_ID += 1;
        }

        public void SetInfo(BuildingConfig buildingConfig)
        {
            columns = buildingConfig.columns;
            rows = buildingConfig.rows;

            if (this.level == 0) this.level = 1;
            var battleData = buildingConfig.levelBattleBuildingConfig[this.level - 1];
            storage = battleData.storage;
            boost = battleData.boost;
            health = battleData.health;
            damage = battleData.damage;
            capacity = battleData.capacity;
            speed = battleData.speed;
            radius = battleData.radius;
            constructionTime = battleData.constructionTime;
            isConstructing = battleData.isConstructing;
            targetType = battleData.targetType;
            blindRange = battleData.blindRange; // Vùng mù
            splashRange = battleData.splashRange;
            rangedSpeed = battleData.rangedSpeed;
            percentage = battleData.percentage;
        }
    }

    public enum BuildingTargetType
    {
        none = 0, ground = 1, air = 2, all = 3
    }
}
