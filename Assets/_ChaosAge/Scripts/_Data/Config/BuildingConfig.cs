using System;

namespace ChaosAge.Config
{
    [Serializable]
    public class BuildingConfig
    {
        EBuildingType type;
        public int requireGold;
        public int requireElixir;
        public int requireGem;

    }

    [Serializable]
    public enum EBuildingType
    {
        townhall, goldmine, goldstorage, elixirmine, elixirstorage, darkelixirmine, darkelixirstorage, buildershut, armycamp, barracks, wall, cannon, archertower, mortor, airdefense, wizardtower, hiddentesla, bombtower, xbow, infernotower, decoration, obstacle, boomb, springtrap, airbomb, giantbomb, seekingairmine, skeletontrap
    }
}

