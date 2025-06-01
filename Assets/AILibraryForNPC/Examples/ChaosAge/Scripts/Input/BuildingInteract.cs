using System;
using ChaosAge.building;
using ChaosAge.Managers;
using UnityEngine;

namespace ChaosAge.input
{
    public class BuildingInteract : MonoBehaviour
    {
        private Vector3 _startTouchPos;

        private void Start()
        {
            TouchControllerInCity.Instance.OnTouchStarted += OnTouchStarted;
            TouchControllerInCity.Instance.OnTouchMoved += OnTouchMoved;
            TouchControllerInCity.Instance.OnTouchEnded += OnTouchEnded;
            TouchControllerInCity.Instance.OnTap += OnTap;
        }

        private void OnTouchEnded(Vector3 vector)
        {
            throw new NotImplementedException();
        }

        private void OnTap(Vector3 worldPos)
        {
            GameObject hit = RaycastBuilding(worldPos);
            if (hit != null)
            {
                SelectBuilding(hit);
                TouchControllerInCity.Instance.TouchState = TouchState.MovingBuilding;
            }
            else
            {
                DeselectBuilding();
            }
        }

        private void OnTouchMoved(Vector3 delta)
        {
            // var building = BuildingManager.Instance.SelectedBuilding;
            // if (building != null)
            // {
            //     building.transform.position = _startTouchPos + delta;
            // }
        }

        public void OnTouchStarted(Vector3 touchPos)
        {
            // var building = BuildingManager.Instance.SelectedBuilding;
            // if (building != null)
            // {
            //     _startTouchPos = touchPos;
            // }
        }

        GameObject RaycastBuilding(Vector3 worldPos)
        {
            // Ray từ camera về worldPos trên mặt đất
            Ray ray = new Ray(
                Camera.main.transform.position,
                worldPos - Camera.main.transform.position
            );
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.CompareTag("Building"))
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        void SelectBuilding(GameObject building)
        {
            // BuildingManager.Instance.SelectedBuilding = building.GetComponent<Building0>();
            // Debug.Log("Selected building: " + building.name);
            // // TODO: highlight hoặc hiển thị UI nếu muốn
        }

        void DeselectBuilding()
        {
            // if (BuildingManager.Instance.SelectedBuilding != null)
            // {
            //     Debug.Log("Deselected building: " + BuildingManager.Instance.SelectedBuilding.name);
            //     BuildingManager.Instance.SelectedBuilding = null;
            // }
        }
    }
}
