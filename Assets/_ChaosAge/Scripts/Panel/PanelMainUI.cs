using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.panel
{
    public class PanelMainUI : MonoBehaviour
    {
        public static bool IsActive;

        [Header("Resources")]
        [SerializeField] TMP_Text textGold;
        [SerializeField] TMP_Text textElixir;
        [SerializeField] TMP_Text textGem;

        [Header("Buttons")]
        [SerializeField] Button btnShop;
        [SerializeField] Button btnBattle;


        // Start is called before the first frame update
        void Start()
        {
            Setup();

        }

        private void Setup()
        {
            btnShop.onClick.AddListener(ShopButtonClicked);
        }

        private void ShopButtonClicked()
        {

        }
    }

}
