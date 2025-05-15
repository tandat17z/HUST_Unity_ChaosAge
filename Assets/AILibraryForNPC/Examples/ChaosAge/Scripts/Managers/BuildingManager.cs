using ChaosAge.building;
using DatSystem.utils;

namespace ChaosAge.Managers
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        public Building SelectedBuilding { get; set; }

        protected override void OnAwake()
        {

        }
    }
}
