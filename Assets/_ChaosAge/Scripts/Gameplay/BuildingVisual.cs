using System.Linq;
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

        [Header("Visual Settings")]
        [SerializeField]
        private StatusAndMaterial[] materials;

        private Building building;

        private void Awake()
        {
            if (building == null)
                building = GetComponent<Building>();
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
    }
}
