using ChaosAge.Config;
using ChaosAge.Data;
using DatSystem.utils;

namespace DatSystem
{
    public class DataManager : Singleton<DataManager>
    {
        private PlayerData _playerData;
        private GameConfig _gameConfig;
        public PlayerData PlayerData { get { return _playerData; } }
        public GameConfig GameConfig { get { return _gameConfig; } }

        public void LoadPlayerData()
        {
            _playerData = PlayerData.Load();
        }

        public void LoadGameConfig()
        {
            _gameConfig = new GameConfig();
        }

        protected override void OnAwake()
        {
        }
    }

}
