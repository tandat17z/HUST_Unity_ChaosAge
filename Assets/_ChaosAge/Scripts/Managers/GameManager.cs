using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] CameraController cameraController;

        private GameState _gameState;

        public GameState GameState { get { return _gameState; } }

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
            BuildingManager.Instance.LoadMap(_playerData.buildings);


            // open UI
            PanelManager.Instance.OpenPanel<PanelMainUI>();
        }
        #endregion

    }

    public enum GameState
    {
        City,
        Battle
    }

}

