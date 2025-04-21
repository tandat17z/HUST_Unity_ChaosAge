using System.Collections.Generic;
using ChaosAge.Config;
using ChaosAge.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChaosAge.AI.battle
{
    public class AIBattleBuilding : MonoBehaviour
    {
        [Header("")]
        [SerializeField]
        EBuildingType type;

        [SerializeField, ReadOnly]
        public BattleBuildingData battleBuidlingConfig = null;

        [SerializeField]
        int level;
        public Vector2Int GridPosition { get; internal set; }
        public int Columns = 3;
        public int Rows = 3;

        public void SetInfo(BuildingData data)
        {
            GridPosition = new Vector2Int(data.x, data.y);
        }
    }
}
