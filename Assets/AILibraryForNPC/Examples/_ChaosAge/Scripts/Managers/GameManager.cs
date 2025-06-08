using System;
using ChaosAge;
using ChaosAge.AI.battle;
using ChaosAge.data;
using DatSystem;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState _gameState = GameState.City;

        public GameState GameState
        {
            get { return _gameState; }
        }

        private PlayerData _playerData;
        private PanelLog _panelLog;

        #region Init
        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            // load data
            DataManager.Instance.LoadPlayerData();
            // DataManager.Instance.LoadGameConfig();

            // load
            _playerData = DataManager.Instance.PlayerData;

            SwitchToCity();

            // open UI
            PanelManager.Instance.OpenPanel<PanelCheat>();
            PanelManager.Instance.OpenPanel<PanelLog>();
        }
        #endregion

        public void SwitchToCity()
        {
            _gameState = GameState.City;
            PanelManager.Instance.ClosePanel<PanelBattle>();
            PanelManager.Instance.OpenPanel<PanelMainUI>();

            var buildingDatas = _playerData.GetListBuildingData();
            BuildingManager.Instance.LoadMap(buildingDatas);
        }

        public void SwitchToBattleAI(int level)
        {
            Log($"SwitchToBattleAI: {level}");
            _gameState = GameState.BattleAI;
            PanelManager.Instance.ClosePanel<PopupSelectLevel>();
            PanelManager.Instance.ClosePanel<PanelMainUI>();
            PanelManager.Instance.OpenPanel<PanelBattle>();

            // load level
            AIBattleManager.Instance.Initialize(level);
        }

        public void Log(string log)
        {
            var panelLog = PanelManager.Instance.GetPanel<PanelLog>();
            panelLog.ShowLog(log);

            Debug.Log(log);
        }
    }

    public enum GameState
    {
        City,
        BattleAI,
    }
}
