using ChaosAge.building;
using ChaosAge.Config;
using ChaosAge.manager;
using DatSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.UI.elements
{
    public class ButtonBuild : MonoBehaviour
    {
        [SerializeField] private EBuildingType buildingType;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text textName;

        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(Clicked);
            textName.text = buildingType.ToString();
        }

        private void Clicked()
        {
            Vector3 position = Vector3.zero;

            Building building = FactoryManager.Instance.SpawnBuilding(buildingType);
            building.SetInfo(-1, 0);
            building.PlacedOnGrid(20, 20);


            BuildingManager.Instance.Select(building);

            PanelManager.Instance.OpenPanel<UIBuild>();
            PanelManager.Instance.ClosePanel<PanelShop>();

        }
    }

}