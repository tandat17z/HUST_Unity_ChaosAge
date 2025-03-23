using System;
using System.Collections.Generic;
using ChaosAge.Config;

namespace ChaosAge.Data
{
    public class PlayerData
    {
        public int Gold;
        public int Elixir;
        public int Gem;

        public List<BuildingData> buildings;

        public PlayerData()
        {
            buildings = new List<BuildingData>();
            buildings.Add(new BuildingData(EBuildingType.TownHall, 20, 20));
            buildings.Add(new BuildingData(EBuildingType.BuilderHut, 25, 20));
            buildings.Add(new BuildingData(EBuildingType.GoldMine, 28, 20));
            buildings.Add(new BuildingData(EBuildingType.ArmyCamp, 32, 20));
        }

        public void AddBuiling(BuildingData buildingData)
        {
            buildings.Add(buildingData);
        }

        public void Save()
        {

        }
    }

}
