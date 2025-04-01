using System;
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
        public EBuildingType buildingType;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text textName;

        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(Clicked);

        }

        public void SetInfo(EBuildingType type)
        {
            buildingType = type;
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

        public void SetInteractable(bool value)
        {
            button.interactable = value;
        }
    }

}