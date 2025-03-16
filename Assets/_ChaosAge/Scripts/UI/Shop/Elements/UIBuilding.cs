using System;
using System.Collections;
using System.Collections.Generic;
using ChaosAge.building;
using ChaosAge.manager;
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

            BuildingManager.Instance.SelectedBuilding = building;
            BuildingManager.Instance.IsPlacingBuilding = true;
        }

        public void ConfirmBuild()
        {
            // send to server
        }
    }

}