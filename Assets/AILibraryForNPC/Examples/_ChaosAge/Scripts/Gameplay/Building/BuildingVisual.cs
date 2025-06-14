using System;
using System.Linq;
using ChaosAge.manager;
using DatSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.building
{
    public class BuildingVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private TextMeshProUGUI levelText;

        [SerializeField]
        private Image slider;

        [SerializeField]
        private TMP_Text sliderText;

        [Header("Build")]
        [SerializeField]
        private Button buttonBuildOk;

        [SerializeField]
        private Button buttonBuildCancel;

        [Header("Visual Settings")]
        [SerializeField]
        private StatusAndMaterial[] materials;

        [SerializeField]
        private GameObject buildingModel;

        [SerializeField]
        private GameObject buildingModelUpgrade;

        [Header("UI")]
        [SerializeField]
        private GameObject infoUI;

        [SerializeField]
        private GameObject battleUI;

        [SerializeField]
        private GameObject buildUI;

        private Building building;

        private void Awake()
        {
            if (building == null)
                building = GetComponent<Building>();

            buttonBuildOk.onClick.AddListener(() =>
            {
                BuildingManager.Instance.OnBuildOk(building);
            });
            buttonBuildCancel.onClick.AddListener(() =>
            {
                BuildingManager.Instance.OnBuildCancel(building);
            });

            HideBuildUI();
            HideInfoUI();
            HideSliderUI();
        }

        public void Init()
        {
            HideBuildUI();
            HideInfoUI();
            HideSliderUI();

            if (building.CheckUpgrading())
            {
                ShowUpgradeUI();
            }
            else
            {
                HideUpgradeUI();
            }
        }

        public void ShowUpgradeUI()
        {
            buildingModelUpgrade.SetActive(true);
            battleUI.SetActive(true);
        }

        public void HideUpgradeUI()
        {
            buildingModelUpgrade.SetActive(false);
            battleUI.SetActive(false);
        }

        public void SetTime(float remainingTime, float duration)
        {
            sliderText.text = $"{(int)remainingTime}s";
            slider.fillAmount = remainingTime / duration;
        }

        private void UpdateSlider(float percentage)
        {
            slider.fillAmount = percentage;
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
            buildUI.SetActive(true);
        }

        public void HideBuildUI()
        {
            buildUI.SetActive(false);
        }

        public void ShowInfoUI()
        {
            UpdateLevelDisplay();
            infoUI.SetActive(true);
        }

        public void HideInfoUI()
        {
            infoUI.SetActive(false);
        }

        public void ShowSliderUI()
        {
            battleUI.SetActive(true);
        }

        public void HideSliderUI()
        {
            battleUI.SetActive(false);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            var building = GetComponent<Building>();
            nameText.text = building.Type.ToString();
        }

        public void SetSize(Vector2 size)
        {
            meshRenderer.transform.localScale = new Vector3(size.x, size.y, 1);
        }
#endif
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
