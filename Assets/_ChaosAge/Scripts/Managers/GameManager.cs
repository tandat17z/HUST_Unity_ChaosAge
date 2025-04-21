using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.AI.battle;
using ChaosAge.camera;
using ChaosAge.Data;
using ChaosAge.input;
using DatSystem;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        CameraController cameraController;

        private GameState _gameState = GameState.City;

        public GameState GameState
        {
            get { return _gameState; }
        }

        private PlayerData _playerData;

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
        }
        #endregion

        public void SwitchToCity()
        {
            _gameState = GameState.City;
            BattleManager.Instance.Reset();
            BuildingManager.Instance.LoadMap(_playerData.buildings);

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
    }

    public enum GameState
    {
        City,
        Battle,
        BattleAI,
    }
}
