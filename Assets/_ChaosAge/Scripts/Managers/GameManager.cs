using System;
using ChaosAge;
using ChaosAge.AI.battle;
using ChaosAge.Config;
using ChaosAge.Data;
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
            DataManager.Instance.LoadGameConfig();

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
            BattleManager.Instance.Reset();

            var buildingDatas = _playerData.GetListBuildingData();
            ChaosAge.BuildingManager.Instance.LoadMap(buildingDatas);

            PanelManager.Instance.OpenPanel<PanelMainUI>();
        }

        public void SwitchToBattle()
        {
            _gameState = GameState.Battle;
            PanelManager.Instance.ClosePanel<UIBuildingInfo>();
            PanelManager.Instance.ClosePanel<PanelMainUI>();

            PanelManager.Instance.OpenPanel<PanelBattle>();

            BattleManager.Instance.LoadLevel(0);
        }

        public void SwitchToBattleAI()
        {
            _gameState = GameState.BattleAI;
            PanelManager.Instance.ClosePanel<UIBuildingInfo>();
            PanelManager.Instance.ClosePanel<PanelMainUI>();

            PanelManager.Instance.OpenPanel<PanelBattle>();

            AIBattleManager.Instance.LoadLevel(0);
        }

        public void Log(string log)
        {
            PanelLog.Instance.ShowLog(log);
        }
    }

    public enum GameState
    {
        City,
        Battle,
        BattleAI,
    }
}
