using System;
using System.Linq;
using DatSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge
{
    public class BuildingVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private TextMeshProUGUI levelText;

        [SerializeField]
        private Image healthBar;

        [Header("Build")]
        [SerializeField]
        private Button buttonBuildOk;

        [SerializeField]
        private Button buttonBuildCancel;

        [Header("Visual Settings")]
        [SerializeField]
        private StatusAndMaterial[] materials;

        private Building building;

        private void Awake()
        {
            if (building == null)
                building = GetComponent<Building>();

            HideBuildUI();

            buttonBuildOk.onClick.AddListener(OnBuildOk);
            buttonBuildCancel.onClick.AddListener(OnBuildCancel);
        }

        private void OnBuildOk()
        {
            if (BuildingManager.Instance.CheckOverlapBuilding(building))
            {
                Debug.LogWarning("Overlap building");
            }
            else
            {
                Debug.Log("Build building");
                building.StopMoving();
                building.IsBuilding = false;
                HideBuildUI();

                // save building data
                var newBuildingData = building.GetData();
                newBuildingData.Save();
                DataManager.Instance.PlayerData.AddBuilding(newBuildingData);
            }
        }

        private void OnBuildCancel()
        {
            BuildingManager.Instance.DeselectBuilding();
            Destroy(building.gameObject);
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
            {
                float healthPercentage = (float)building.Health / 100f;
                healthBar.fillAmount = healthPercentage;
            }
        }

        private void UpdateLevelDisplay()
        {
            if (levelText != null)
            {
                levelText.text = $"Lv.{building.Level}";
            }
        }

        public void OnBuildingSelected()
        {
            meshRenderer.material = GetMaterial(EBuildingStatus.Selected);
        }

        public void OnBuildingDeselected()
        {
            meshRenderer.material = GetMaterial(EBuildingStatus.Normal);
        }

        private Material GetMaterial(EBuildingStatus status)
        {
            var mat = materials.FirstOrDefault(m => m.status == status);
            if (mat == null)
            {
                Debug.LogError($"Material for state {status} not found");
                return null;
            }
            return mat.material;
        }

        public void OnBuildingOverlap()
        {
            meshRenderer.material = GetMaterial(EBuildingStatus.Overlap);
        }

        public void ShowBuildUI()
        {
            buttonBuildOk.gameObject.SetActive(true);
            buttonBuildCancel.gameObject.SetActive(true);
        }

        public void HideBuildUI()
        {
            buttonBuildOk.gameObject.SetActive(false);
            buttonBuildCancel.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public class StatusAndMaterial
    {
        public EBuildingStatus status;
        public Material material;
    }

    public enum EBuildingStatus
    {
        Normal,
        Selected,
        Overlap,
    }
}
