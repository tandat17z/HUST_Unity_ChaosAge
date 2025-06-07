using ChaosAge.data;
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
            Debug.Log($"Click load from file");
            if (int.TryParse(inputLevel.text, out var level))
            {
                // var buildings = PlayerData.LoadFromFile($"Assets/Levels/{level}.json").buildings;
                // BuildingManager.Instance.LoadMap(buildings);

                Debug.Log($"Load from file: {level}");
            }
        });
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
    }
}
