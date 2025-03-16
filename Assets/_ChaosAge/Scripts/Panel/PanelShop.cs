using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosAge.panel
{
    public class PanelShop : MonoBehaviour
    {
        [SerializeField] Button btnClose;

        // Start is called before the first frame update
        void Start()
        {
            Setup();
        }

        void Setup()
        {
            btnClose.onClick.AddListener(CloseShop);
        }

        private void CloseShop()
        {

        }

    }
}
