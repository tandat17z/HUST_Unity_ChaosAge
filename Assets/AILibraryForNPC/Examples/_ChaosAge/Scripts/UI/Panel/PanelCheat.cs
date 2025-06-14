using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChaosAge.data;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelCheat : Panel
{
    [Header("Save/load json")]
    [SerializeField]
    Button saveToJson;

    [SerializeField]
    TMP_InputField inputLevel;

    [SerializeField]
    Button loadFromJson;
    private PlayerData _playerData;

    public override void OnSetup()
    {
        base.OnSetup();

        saveToJson.onClick.AddListener(() =>
        {
            if (int.TryParse(inputLevel.text, out var level))
            {
                _playerData.SaveToFile($"Assets/Levels/{level}.json");
            }
        });

        loadFromJson.onClick.AddListener(() =>
        {
            if (int.TryParse(inputLevel.text, out var level))
            {
                var filePath = $"Assets/Levels/{level}.json";
                string json = File.ReadAllText(filePath);
                var buildingFile = JsonUtility.FromJson<PlayerData.BuildingFile>(json);

                // lưu data hiện tại vào playerdata
                SaveToPlayerData(buildingFile.listBuilding);

                BuildingManager.Instance.LoadMap(buildingFile.listBuilding);
                Debug.Log($"Load from file: {level}");
            }
        });
    }

    private void SaveToPlayerData(List<BuildingData> buildingDatas)
    {
        foreach (var buildingData in buildingDatas)
        {
            buildingData.Save();
        }

        var playerData = DataManager.Instance.PlayerData;
        playerData.buildingIds = buildingDatas.Select(b => b.id).ToList();
        playerData.Save();
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        _playerData = DataManager.Instance.PlayerData;
    }

    private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                _playerData.AddResource(EResourceType.Gold, 5);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _playerData.AddResource(EResourceType.Elixir, 5);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                _playerData.AddUnit(EUnitType.Barbarian, 1);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _playerData.AddUnit(EUnitType.Archer, 1);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _playerData.ReduceUnit(EUnitType.Barbarian, 1000);
                _playerData.ReduceUnit(EUnitType.Archer, 1000);
            }
        }
}
