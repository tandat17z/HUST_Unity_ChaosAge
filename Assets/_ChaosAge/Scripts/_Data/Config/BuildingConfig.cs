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
        townhall, goldmine, goldstorage, elixirmine, elixirstorage, darkelixirmine, darkelixirstorage, buildershut, armycamp, barracks, wall, cannon, archertower, mortor, airdefense, wizardtower, hiddentesla, bombtower, xbow, infernotower, decoration, obstacle, boomb, springtrap, airbomb, giantbomb, seekingairmine, skeletontrap
    }
}

