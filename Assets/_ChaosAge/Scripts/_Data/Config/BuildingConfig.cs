using System;

namespace ChaosAge.Config
{
    public class BuildingConfig
    {
        internal int requireGold;
        internal int requireElixir;
        internal int requireGem;
        EBuildingType buildingType;

    }

    [Serializable]
    public enum EBuildingType
    {
        TownHall,
        GoldMine,
        ArmyCamp,
        BuilderHut
    }
}

