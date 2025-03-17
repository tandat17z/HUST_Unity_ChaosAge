using System.Collections;
using System.Collections.Generic;
using DatSystem.UI;
using DatSystem.utils;
using UnityEngine;

namespace ChaosAge.manager
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState _gameState;

        public GameState GameState { get { return _gameState; } }


        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            yield return new WaitForFixedUpdate();

            // load data
            DataManager.Instance.LoadPlayerData();
            DataManager.Instance.LoadGameConfig();

            // open UI
            PanelManager.Instance.OpenPanel<PanelMainUI>();
        }

    }

    public enum GameState
    {
        City,
        Battle
    }

}

