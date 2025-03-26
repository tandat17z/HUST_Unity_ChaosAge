using System.Collections.Generic;
using ChaosAge.Data;
using ChaosAge.input;
using ChaosAge.manager;
using DatSystem;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelBattle : Panel
{
    [SerializeField] Button btnClose;

    public static ButtonUnit SelectedButtonUnit;

    private bool _isActive = false;

    public override void OnSetup()
    {
        base.OnSetup();

        btnClose.onClick.AddListener(Close);
    }

    public override void Open(UIData uiData)
    {
        base.Open(uiData);
        _isActive = true;

        var listBuilding = BuildingManager.Instance.Buildings;
        var listBattleBuilding = new List<BattleBuilding>();
        foreach (var item in listBuilding)
        {
            var battleBuilding = item.GetComponent<BattleBuilding>();
            battleBuilding.building.id = item.GetData().id;
            battleBuilding.building.x = item.GetData().x;
            battleBuilding.building.y = item.GetData().y;
            listBattleBuilding.Add(battleBuilding);
        }
        BattleManager.Instance.Initialize(listBattleBuilding);
    }

    public override void Close()
    {
        _isActive = false;
        base.Close();
    }

    private void Update()
    {
        if (_isActive)
        {

            if (Input.GetMouseButtonDown(0) && SelectedButtonUnit != null)
            {
                var pos = InputHandler.Instance.GetPointerPositionInMap();
                var posCell = BuildingManager.Instance.Grid.ConvertGridPos(pos);
                Debug.Log(posCell);
                BattleManager.Instance.AddUnit(SelectedButtonUnit.Type, (int)posCell.x, (int)posCell.y);
            }
        }
    }
}
