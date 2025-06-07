using System;

namespace ChaosAge.data
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
        public float moveSpeed = 1;
        public float attackSpeed = 1;
        public float attackRange = 1;
        public float damage = 1;
        public float splashRange = 0;
        public float rangedSpeed = 5;
        public TargetPriority priority = TargetPriority.all;
        public UnitMoveType movement = UnitMoveType.ground;
        public float priorityMultiplier = 1;
    }

    [Serializable]
    public enum EUnitType
    {
        barbarian,
        archer,
        // goblin,
        // healer,
        // wallbreaker,
        // giant,
        // miner,
        // balloon,
        // wizard,
        // dragon,
        // pekka,
        // AIAgent,
        // RLAgent,
        // QLearningBarbarian,
        // GOAPBarbarian,
    }

    [Serializable]
    public enum TargetPriority
    {
        none = 0,
        all = 1,
        defenses = 2,
        resources = 3,
        walls = 4,
    }

    [Serializable]
    public enum UnitMoveType
    {
        ground = 0,
        jump = 1,
        fly = 2,
        underground = 3,
    }
}
