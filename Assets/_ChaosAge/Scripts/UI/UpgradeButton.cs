
namespace ChaosAge.UI.elements
{
    using ChaosAge.manager;
    using DatSystem.UI;

    public class UpgradeButton : BaseActionButton
    {
        public override void Active()
        {
            var building = BuildingManager.Instance.SelectedBuilding;
            if (building != null)
            {
                building.Upgrade();

                BuildingManager.Instance.Unselect();
                PanelManager.Instance.ClosePanel<UIBuildingInfo>();
            }
        }
    }
}

