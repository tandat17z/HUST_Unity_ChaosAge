using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.Battle
{
    public class VisualUnit : MonoBehaviour
    {
        [SerializeField]
        Image hpSlider;

        [SerializeField]
        TMP_Text hpText;

        [SerializeField]
        TMP_Text textName;

        private BattleUnit _battleUnit;

        public void Awake()
        {
            _battleUnit = GetComponent<BattleUnit>();
            textName.text = _battleUnit.Type.ToString();
        }

        public void SetHealth(int health, int maxHealth)
        {
            hpSlider.fillAmount = (float)health / maxHealth;
            hpText.text = $"{health}";
        }
    }
}
