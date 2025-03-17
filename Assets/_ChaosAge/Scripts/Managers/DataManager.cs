using System.Collections;
using System.Collections.Generic;
using DatSystem.utils;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private PlayerData _playerData;

    public PlayerData PlayerData { get { return _playerData; } }

    public void LoadPlayerData()
    {
        _playerData = new PlayerData();
    }

    public void LoadGameConfig()
    {

    }

    protected override void OnAwake()
    {
    }
}
