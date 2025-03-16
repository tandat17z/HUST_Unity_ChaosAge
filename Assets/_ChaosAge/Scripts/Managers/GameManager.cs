using System.Collections;
using System.Collections.Generic;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState _gameState;

        public GameState GameState { get { return _gameState; } }

        void Start()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            Initialize();
        }

        void Initialize()
        {
            DataManager.Instance.LoadPlayerData();
        }
    }

    public enum GameState
    {
        City,
        Battle
    }

}

