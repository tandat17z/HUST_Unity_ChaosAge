using System;

namespace ChaosAge.Data
{
    [Serializable]
    public class UnitData
    {
        public EUnitType type;
        public int level;
        public int hosing = 1;
        public bool trained = false;
        public bool ready = false;
        public int health = 0;
        public int trainTime = 0;
        public int trainedTime = 0;

    }

    [Serializable]
    public enum EUnitType
    {
        Barbarian,
        Archer
    }
}

