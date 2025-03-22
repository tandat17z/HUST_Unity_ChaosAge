using ChaosAge.building;
using ChaosAge.manager;
using DatSystem.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.UI.elements
{
    public class UIBuilding : MonoBehaviour
    {
        [SerializeField] private int prefabIndex = 0;
        [SerializeField] private Button button;

        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            Vector3 position = Vector3.zero;

            var prefab = BuildingManager.Instance.Prefabs[prefabIndex];
            Building building = Instantiate(prefab, position, Quaternion.identity);
            building.PlacedOnGrid(20, 20);

            BuildingManager.Instance.Select(building);

            PanelManager.Instance.OpenPanel<UIBuild>();
            PanelManager.Instance.ClosePanel<PanelShop>();

        }
    }

}