using System;
using System.Collections.Generic;

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
        }

        public void AddBuiling(BuildingData buildingData)
        {
            buildings.Add(buildingData);
        }
    }

}
