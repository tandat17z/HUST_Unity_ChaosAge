using ChaosAge.data;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelBattle : Panel
{
    [SerializeField]
    Button btnClose;

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
    }

    public override void Close()
    {
        _isActive = false;

        GameManager.Instance.SwitchToCity();
        base.Close();
    }

    //private void Update()
    //{
    //    if (_isActive)
    //    {

    //        if (Input.GetMouseButtonDown(0) && SelectedButtonUnit != null)
    //        {
    //            var pos = InputHandler.Instance.GetPointerPositionInMap();
    //            var posCell = BuildingManager.Instance.Grid.ConvertGridPos(pos);
    //            BattleManager.Instance.AddUnit(SelectedButtonUnit.Type, (int)posCell.x, (int)posCell.y);
    //        }
    //    }
    //}

    public EUnitType GetCurrentBuildingType()
    {
        if (SelectedButtonUnit == null)
            return EUnitType.barbarian;
        return SelectedButtonUnit.Type;
    }
}
